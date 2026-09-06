export const PriorityLevel = {
  Low: 1,
  Medium: 2,
  High: 3,
  Urgent: 4,
} as const;
export type PriorityLevel = (typeof PriorityLevel)[keyof typeof PriorityLevel];

export const RecurrenceType = {
  None: 0,
  Daily: 1,
  Weekly: 2,
  Monthly: 3,
  SpecificWeekdays: 4,
} as const;
export type RecurrenceType = (typeof RecurrenceType)[keyof typeof RecurrenceType];

export const HabitFrequency = {
  Daily: 1,
  Weekly: 2,
  Monthly: 3,
} as const;
export type HabitFrequency = (typeof HabitFrequency)[keyof typeof HabitFrequency];

export interface TaskItem {
  id: string;
  title: string;
  description: string;
  priority: PriorityLevel;
  priorityName: string;
  categoryId?: number;
  dueDate?: string;
  estimatedMinutes?: number;
  isCompleted: boolean;
  isOverdue: boolean;
  recurrence: RecurrenceType;
  createdAtUtc: string;
  updatedAtUtc?: string;
}

export interface TaskQueryFilter {
  priority?: PriorityLevel;
  categoryId?: number;
  isCompleted?: boolean;
  search?: string;
  sortBy?: string;
  sortDescending?: boolean;
  pageNumber?: number;
  pageSize?: number;
}

export interface PagedResponse<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface HabitItem {
  id: string;
  name: string;
  description: string;
  frequency: HabitFrequency;
  targetPerPeriod: number;
  currentStreak: number;
  longestStreak: number;
  completionRate: number;
  isCompletedToday: boolean;
  createdAtUtc: string;
  completionDates: string[];
}

export interface CalendarDay {
  date: string;
  completedTaskCount: number;
  totalTaskCount: number;
  completionRate: number;
  tasks: TaskItem[];
}

export interface CalendarMonth {
  year: number;
  month: number;
  monthName: string;
  days: CalendarDay[];
}

export interface WeeklyDayStat {
  dayOfWeek: string;
  totalTasks: number;
  completedTasks: number;
  completionRate: number;
}

export interface StatisticsOverview {
  totalTasks: number;
  completedTasks: number;
  incompleteTasks: number;
  completionRate: number;
  activeHabitStreaks: number;
  bestDay: string;
  worstDay: string;
  weeklyBreakdown: WeeklyDayStat[];
}
