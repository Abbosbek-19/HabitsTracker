import React, { useState } from 'react';
import { PriorityLevel, type TaskItem } from '../types';
import { Search, Plus, Trash2, Check, Clock } from 'lucide-react';

interface TasksViewProps {
  tasks: TaskItem[];
  onToggleTask: (task: TaskItem) => void;
  onDeleteTask: (id: string) => void;
  onOpenCreateTask: () => void;
  onSearchChange: (search: string) => void;
  onPriorityFilterChange: (priority?: PriorityLevel) => void;
}

export const TasksView: React.FC<TasksViewProps> = ({
  tasks,
  onToggleTask,
  onDeleteTask,
  onOpenCreateTask,
  onSearchChange,
  onPriorityFilterChange,
}) => {
  const [searchTerm, setSearchTerm] = useState('');
  const [priorityFilter, setPriorityFilter] = useState<string>('all');

  const handleSearch = (val: string) => {
    setSearchTerm(val);
    onSearchChange(val);
  };

  const handlePriorityFilter = (val: string) => {
    setPriorityFilter(val);
    if (val === 'all') {
      onPriorityFilterChange(undefined);
    } else {
      onPriorityFilterChange(Number(val) as PriorityLevel);
    }
  };

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1.5rem' }}>
        <div>
          <h1 style={{ fontSize: '1.75rem', fontWeight: 800 }}>Task Management</h1>
          <p style={{ color: 'var(--text-muted)' }}>Manage, filter, and track all your daily responsibilities.</p>
        </div>
        <button className="btn-primary" onClick={onOpenCreateTask}>
          <Plus size={18} /> Add Task
        </button>
      </div>

      <div className="glass-card" style={{ marginBottom: '1.5rem', padding: '1rem 1.5rem' }}>
        <div style={{ display: 'flex', gap: '1rem', flexWrap: 'wrap' }}>
          <div style={{ flex: 1, minWidth: '200px', display: 'flex', alignItems: 'center', gap: '0.5rem', background: 'var(--bg-main)', border: '1px solid var(--border-color)', borderRadius: 'var(--radius-sm)', padding: '0.5rem 0.75rem' }}>
            <Search size={18} color="var(--text-muted)" />
            <input
              type="text"
              placeholder="Search tasks..."
              value={searchTerm}
              onChange={(e) => handleSearch(e.target.value)}
              style={{ border: 'none', background: 'transparent', outline: 'none', color: 'var(--text-main)', width: '100%' }}
            />
          </div>

          <select
            className="form-control"
            style={{ width: 'auto' }}
            value={priorityFilter}
            onChange={(e) => handlePriorityFilter(e.target.value)}
          >
            <option value="all">All Priorities</option>
            <option value={PriorityLevel.Urgent}>Urgent ⚡</option>
            <option value={PriorityLevel.High}>High</option>
            <option value={PriorityLevel.Medium}>Medium</option>
            <option value={PriorityLevel.Low}>Low</option>
          </select>
        </div>
      </div>

      <div className="glass-card">
        {tasks.length === 0 ? (
          <div style={{ textAlign: 'center', padding: '3rem 0', color: 'var(--text-muted)' }}>
            <p style={{ fontSize: '1.1rem', fontWeight: 600 }}>No tasks found matching your filter.</p>
            <p style={{ fontSize: '0.85rem', marginTop: '0.3rem' }}>Try searching or create a new task!</p>
          </div>
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
                <span className={`badge-priority priority-${(task.priorityName || 'medium').toLowerCase()}`}>
                  {task.priorityName || 'Medium'}
                </span>

                <button
                  onClick={() => onDeleteTask(task.id)}
                  style={{
                    background: 'none',
                    border: 'none',
                    color: 'var(--danger-color)',
                    cursor: 'pointer',
                    padding: '0.3rem',
                  }}
                >
                  <Trash2 size={16} />
                </button>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
};
