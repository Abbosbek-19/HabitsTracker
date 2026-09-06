import React from 'react';
import { PriorityLevel, type HabitItem, type TaskItem } from '../types';
import { Check, Flame, Clock } from 'lucide-react';

interface DashboardViewProps {
  tasks: TaskItem[];
  habits: HabitItem[];
  onToggleTask: (task: TaskItem) => void;
  onRecordHabit: (habitId: string) => void;
  onOpenCreateTask: () => void;
}

export const DashboardView: React.FC<DashboardViewProps> = ({
  tasks,
  habits,
  onToggleTask,
  onRecordHabit,
  onOpenCreateTask,
}) => {
  const totalTasks = tasks.length;
  const completedTasks = tasks.filter((t) => t.isCompleted).length;
  const completionPercentage = totalTasks > 0 ? Math.round((completedTasks / totalTasks) * 100) : 0;
  const todayFormatted = new Date().toLocaleDateString('en-US', {
    weekday: 'long',
    month: 'long',
    day: 'numeric',
    year: 'numeric',
  });

  const getPriorityBadge = (priority: PriorityLevel) => {
    switch (priority) {
      case PriorityLevel.Urgent:
        return <span className="badge-priority priority-urgent">Urgent ⚡</span>;
      case PriorityLevel.High:
        return <span className="badge-priority priority-high">High</span>;
      case PriorityLevel.Medium:
        return <span className="badge-priority priority-medium">Medium</span>;
      case PriorityLevel.Low:
        return <span className="badge-priority priority-low">Low</span>;
      default:
        return <span className="badge-priority priority-medium">Medium</span>;
    }
  };

  return (
    <div>
      <div className="grid-dashboard">
        <div className="glass-card" style={{ gridColumn: 'span 2' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <div>
              <h2 style={{ fontSize: '1.4rem', fontWeight: 800 }}>Good morning! 👋</h2>
              <p style={{ color: 'var(--text-muted)', marginTop: '0.2rem' }}>{todayFormatted}</p>
            </div>
            <button className="btn-primary" onClick={onOpenCreateTask}>
              ➕ Add New Task
            </button>
          </div>

          <div style={{ marginTop: '1.5rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', fontWeight: 600 }}>
              <span>Today's Progress</span>
              <span style={{ color: 'var(--success-color)' }}>{completionPercentage}%</span>
            </div>

            <div className="progress-bar-container">
              <div className="progress-bar-fill" style={{ width: `${completionPercentage}%` }}></div>
            </div>

            <span style={{ fontSize: '0.85rem', color: 'var(--text-muted)' }}>
              {completedTasks} / {totalTasks} tasks completed
            </span>
          </div>
        </div>

        <div className="glass-card">
          <h3 style={{ fontSize: '1.1rem', fontWeight: 700, marginBottom: '1rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
            <Flame color="#ef4444" fill="#ef4444" size={20} /> Active Streaks
          </h3>

          {habits.length === 0 ? (
            <p style={{ fontSize: '0.85rem', color: 'var(--text-muted)' }}>No habits created yet.</p>
          ) : (
            <div style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
              {habits.map((habit) => (
                <div
                  key={habit.id}
                  style={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'space-between',
                    padding: '0.75rem',
                    borderRadius: 'var(--radius-sm)',
                    backgroundColor: 'rgba(255,255,255,0.05)',
                    border: '1px solid var(--border-color)',
                  }}
                >
                  <div>
                    <div style={{ fontWeight: 600, fontSize: '0.9rem' }}>{habit.name}</div>
                    <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>
                      🔥 {habit.currentStreak} day streak
                    </div>
                  </div>
                  <button
                    onClick={() => onRecordHabit(habit.id)}
                    style={{
                      padding: '0.4rem 0.8rem',
                      borderRadius: 'var(--radius-sm)',
                      border: 'none',
                      backgroundColor: habit.isCompletedToday ? 'var(--success-color)' : 'var(--accent-primary)',
                      color: 'white',
                      fontWeight: 600,
                      fontSize: '0.75rem',
                      cursor: 'pointer',
                    }}
                  >
                    {habit.isCompletedToday ? '✓ Completed' : 'Check-in'}
                  </button>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>

      <div className="glass-card">
        <h3 style={{ fontSize: '1.2rem', fontWeight: 700, marginBottom: '1rem' }}>Today's Tasks</h3>

        {tasks.length === 0 ? (
          <p style={{ color: 'var(--text-muted)', padding: '1rem 0' }}>No tasks created for today.</p>
        ) : (
          tasks.map((task) => (
            <div key={task.id} className="task-row">
              <div className="task-left">
                <div
                  className={`checkbox-custom ${task.isCompleted ? 'completed' : ''}`}
                  onClick={() => onToggleTask(task)}
                >
                  {task.isCompleted && <Check size={14} />}
                </div>
                <div>
                  <div
                    style={{
                      fontWeight: 600,
                      textDecoration: task.isCompleted ? 'line-through' : 'none',
                      color: task.isCompleted ? 'var(--text-muted)' : 'var(--text-main)',
                    }}
                  >
                    {task.title}
                  </div>
                  {task.description && (
                    <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)', marginTop: '0.1rem' }}>
                      {task.description}
                    </div>
                  )}
                </div>
              </div>

              <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
                {task.estimatedMinutes && (
                  <span style={{ fontSize: '0.8rem', color: 'var(--text-muted)', display: 'flex', alignItems: 'center', gap: '0.3rem' }}>
                    <Clock size={14} /> {task.estimatedMinutes}m
                  </span>
                )}
                {getPriorityBadge(task.priority)}
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
};
