import axios from 'axios';
import type {
  CalendarMonth,
  HabitItem,
  PagedResponse,
  PriorityLevel,
  RecurrenceType,
  StatisticsOverview,
  TaskItem,
  TaskQueryFilter,
} from '../types';

const API_BASE_URL = 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const tasksApi = {
  getPaged: async (filter?: TaskQueryFilter): Promise<PagedResponse<TaskItem>> => {
    const params = new URLSearchParams();
    if (filter?.priority !== undefined) params.append('priority', filter.priority.toString());
    if (filter?.categoryId !== undefined) params.append('categoryId', filter.categoryId.toString());
    if (filter?.isCompleted !== undefined) params.append('isCompleted', filter.isCompleted.toString());
    if (filter?.search) params.append('search', filter.search);
    if (filter?.sortBy) params.append('sortBy', filter.sortBy);
    if (filter?.sortDescending !== undefined) params.append('sortDescending', filter.sortDescending.toString());
    if (filter?.pageNumber) params.append('pageNumber', filter.pageNumber.toString());
    if (filter?.pageSize) params.append('pageSize', filter.pageSize.toString());

    const response = await api.get<PagedResponse<TaskItem>>(`/tasks?${params.toString()}`);
    return response.data;
  },

  getById: async (id: string): Promise<TaskItem> => {
    const response = await api.get<TaskItem>(`/tasks/${id}`);
    return response.data;
  },

  create: async (data: {
    title: string;
    description: string;
    priority: PriorityLevel;
    dueDate?: string;
    estimatedMinutes?: number;
    categoryId?: number;
    recurrence?: RecurrenceType;
  }): Promise<TaskItem> => {
    const response = await api.post<TaskItem>('/tasks', data);
    return response.data;
  },

  update: async (
    id: string,
    data: {
      title: string;
      description: string;
      priority: PriorityLevel;
      dueDate?: string;
      estimatedMinutes?: number;
      categoryId?: number;
      recurrence?: RecurrenceType;
    }
  ): Promise<TaskItem> => {
    const response = await api.put<TaskItem>(`/tasks/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/tasks/${id}`);
  },

  complete: async (id: string): Promise<TaskItem> => {
    const response = await api.patch<TaskItem>(`/tasks/${id}/complete`);
    return response.data;
  },

  uncomplete: async (id: string): Promise<TaskItem> => {
    const response = await api.patch<TaskItem>(`/tasks/${id}/uncomplete`);
    return response.data;
  },
};

export const habitsApi = {
  getAll: async (): Promise<HabitItem[]> => {
    const response = await api.get<HabitItem[]>('/habits');
    return response.data;
  },

  getById: async (id: string): Promise<HabitItem> => {
    const response = await api.get<HabitItem>(`/habits/${id}`);
    return response.data;
  },

  create: async (data: {
    name: string;
    description: string;
    frequency?: number;
    targetPerPeriod?: number;
  }): Promise<HabitItem> => {
    const response = await api.post<HabitItem>('/habits', data);
    return response.data;
  },

  update: async (
    id: string,
    data: {
      name: string;
      description: string;
      frequency: number;
      targetPerPeriod: number;
    }
  ): Promise<HabitItem> => {
    const response = await api.put<HabitItem>(`/habits/${id}`, data);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/habits/${id}`);
  },

  recordCompletion: async (id: string, date?: string): Promise<HabitItem> => {
    const url = date ? `/habits/${id}/complete?date=${date}` : `/habits/${id}/complete`;
    const response = await api.post<HabitItem>(url);
    return response.data;
  },
};

export const calendarApi = {
  getMonthly: async (year: number, month: number): Promise<CalendarMonth> => {
    const response = await api.get<CalendarMonth>(`/calendar/${year}/${month}`);
    return response.data;
  },
};

export const statisticsApi = {
  getOverview: async (): Promise<StatisticsOverview> => {
    const response = await api.get<StatisticsOverview>('/statistics/overview');
    return response.data;
  },
};
