import { useEffect, useState } from 'react';
import { Sidebar } from './components/Sidebar';
import { DashboardView } from './components/DashboardView';
import { TasksView } from './components/TasksView';
import { HabitsView } from './components/HabitsView';
import { CalendarView } from './components/CalendarView';
import { StatisticsView } from './components/StatisticsView';
import { CreateTaskModal } from './components/CreateTaskModal';
import { CreateHabitModal } from './components/CreateHabitModal';
import type { HabitItem, PriorityLevel, RecurrenceType, TaskItem } from './types';
import { habitsApi, tasksApi } from './api/client';
import { Moon, Sun } from 'lucide-react';

export function App() {
  const [currentTab, setCurrentTab] = useState('dashboard');
  const [theme, setTheme] = useState<'light' | 'dark'>('dark');
  const [tasks, setTasks] = useState<TaskItem[]>([]);
  const [habits, setHabits] = useState<HabitItem[]>([]);
  const [isTaskModalOpen, setIsTaskModalOpen] = useState(false);
  const [isHabitModalOpen, setIsHabitModalOpen] = useState(false);

  useEffect(() => {
    document.documentElement.setAttribute('data-theme', theme);
  }, [theme]);

  useEffect(() => {
    fetchInitialData();
  }, []);

  const fetchInitialData = async () => {
    try {
      const tasksPaged = await tasksApi.getPaged();
      setTasks(tasksPaged.items);

      const habitsData = await habitsApi.getAll();
      setHabits(habitsData);
    } catch (err) {
      console.error('API Fetch Error:', err);
    }
  };

  const toggleTheme = () => {
    setTheme((prev) => (prev === 'light' ? 'dark' : 'light'));
  };

  const handleToggleTask = async (task: TaskItem) => {
    try {
      const updated = task.isCompleted
        ? await tasksApi.uncomplete(task.id)
        : await tasksApi.complete(task.id);

      setTasks((prev) => prev.map((t) => (t.id === task.id ? updated : t)));
      fetchInitialData();
    } catch (err) {
      console.error(err);
    }
  };

  const handleDeleteTask = async (id: string) => {
    try {
      await tasksApi.delete(id);
      setTasks((prev) => prev.filter((t) => t.id !== id));
    } catch (err) {
      console.error(err);
    }
  };

  const handleRecordHabit = async (habitId: string) => {
    try {
      const updatedHabit = await habitsApi.recordCompletion(habitId);
      setHabits((prev) => prev.map((h) => (h.id === habitId ? updatedHabit : h)));
    } catch (err) {
      console.error(err);
    }
  };

  const handleDeleteHabit = async (id: string) => {
    try {
      await habitsApi.delete(id);
      setHabits((prev) => prev.filter((h) => h.id !== id));
    } catch (err) {
      console.error(err);
    }
  };

  const handleCreateTask = async (taskData: {
    title: string;
    description: string;
    priority: PriorityLevel;
    dueDate?: string;
    estimatedMinutes?: number;
    recurrence?: RecurrenceType;
  }) => {
    const newTask = await tasksApi.create(taskData);
    setTasks((prev) => [newTask, ...prev]);
  };

  const handleCreateHabit = async (habitData: {
    name: string;
    description: string;
    frequency: number;
    targetPerPeriod: number;
  }) => {
    const newHabit = await habitsApi.create(habitData);
    setHabits((prev) => [newHabit, ...prev]);
  };

  const handleSearchTasks = async (search: string) => {
    const result = await tasksApi.getPaged({ search });
    setTasks(result.items);
  };

  const handlePriorityFilterTasks = async (priority?: PriorityLevel) => {
    const result = await tasksApi.getPaged({ priority });
    setTasks(result.items);
  };

  return (
    <div className="app-container">
      <Sidebar currentTab={currentTab} onSelectTab={setCurrentTab} />

      <main className="main-content">
        <header className="top-navbar">
          <div className="user-greeting">
            <h1>Daily Task & Habit Tracker</h1>
            <p>ASP.NET Core REST API + PostgreSQL + React TypeScript</p>
          </div>

          <div className="action-bar">
            <button onClick={toggleTheme} className="theme-toggle-btn">
              {theme === 'dark' ? <Sun size={18} /> : <Moon size={18} />}
              <span>{theme === 'dark' ? 'Light Mode' : 'Dark Mode'}</span>
            </button>
          </div>
        </header>

        {currentTab === 'dashboard' && (
          <DashboardView
            tasks={tasks}
            habits={habits}
            onToggleTask={handleToggleTask}
            onRecordHabit={handleRecordHabit}
            onOpenCreateTask={() => setIsTaskModalOpen(true)}
          />
        )}

        {currentTab === 'tasks' && (
          <TasksView
            tasks={tasks}
            onToggleTask={handleToggleTask}
            onDeleteTask={handleDeleteTask}
            onOpenCreateTask={() => setIsTaskModalOpen(true)}
            onSearchChange={handleSearchTasks}
            onPriorityFilterChange={handlePriorityFilterTasks}
          />
        )}

        {currentTab === 'habits' && (
          <HabitsView
            habits={habits}
            onRecordHabit={handleRecordHabit}
            onDeleteHabit={handleDeleteHabit}
            onOpenCreateHabit={() => setIsHabitModalOpen(true)}
          />
        )}

        {currentTab === 'calendar' && <CalendarView />}

        {currentTab === 'statistics' && <StatisticsView />}
      </main>

      <CreateTaskModal
        isOpen={isTaskModalOpen}
        onClose={() => setIsTaskModalOpen(false)}
        onSubmit={handleCreateTask}
      />

      <CreateHabitModal
        isOpen={isHabitModalOpen}
        onClose={() => setIsHabitModalOpen(false)}
        onSubmit={handleCreateHabit}
      />
    </div>
  );
}

export default App;
