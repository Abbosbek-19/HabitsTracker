import React from 'react';
import type { HabitItem } from '../types';
import { Flame, Plus, Trash2, Trophy, Award } from 'lucide-react';

interface HabitsViewProps {
  habits: HabitItem[];
  onRecordHabit: (habitId: string) => void;
  onDeleteHabit: (id: string) => void;
  onOpenCreateHabit: () => void;
}

export const HabitsView: React.FC<HabitsViewProps> = ({
  habits,
  onRecordHabit,
  onDeleteHabit,
  onOpenCreateHabit,
}) => {
  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1.5rem' }}>
        <div>
          <h1 style={{ fontSize: '1.75rem', fontWeight: 800 }}>Habit Tracker</h1>
          <p style={{ color: 'var(--text-muted)' }}>Build long-term consistency and track continuous streaks.</p>
        </div>
        <button className="btn-primary" onClick={onOpenCreateHabit}>
          <Plus size={18} /> New Habit
        </button>
      </div>

      {habits.length === 0 ? (
        <div className="glass-card" style={{ textAlign: 'center', padding: '3rem 0' }}>
          <Flame size={48} color="var(--text-muted)" style={{ margin: '0 auto 1rem' }} />
          <h3 style={{ fontSize: '1.2rem', fontWeight: 700 }}>No Habits Tracked Yet</h3>
          <p style={{ color: 'var(--text-muted)', marginTop: '0.4rem' }}>
            Start building your daily routines by creating a habit!
          </p>
        </div>
      ) : (
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(320px, 1fr))', gap: '1.5rem' }}>
          {habits.map((habit) => (
            <div key={habit.id} className="glass-card" style={{ position: 'relative' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                <div>
                  <h3 style={{ fontSize: '1.15rem', fontWeight: 700 }}>{habit.name}</h3>
                  {habit.description && (
                    <p style={{ fontSize: '0.85rem', color: 'var(--text-muted)', marginTop: '0.2rem' }}>
                      {habit.description}
                    </p>
                  )}
                </div>
                <button
                  onClick={() => onDeleteHabit(habit.id)}
                  style={{ background: 'none', border: 'none', color: 'var(--danger-color)', cursor: 'pointer' }}
                >
                  <Trash2 size={16} />
                </button>
              </div>

              <div style={{ display: 'flex', gap: '1.5rem', margin: '1.5rem 0', padding: '1rem', background: 'rgba(255,255,255,0.04)', borderRadius: 'var(--radius-sm)' }}>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                  <Flame color="#ef4444" fill="#ef4444" size={24} />
                  <div>
                    <div style={{ fontSize: '1.25rem', fontWeight: 800 }}>{habit.currentStreak} Days</div>
                    <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>Current Streak</div>
                  </div>
                </div>

                <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                  <Trophy color="#f59e0b" size={24} />
                  <div>
                    <div style={{ fontSize: '1.25rem', fontWeight: 800 }}>{habit.longestStreak} Days</div>
                    <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>Best Streak</div>
                  </div>
                </div>
              </div>

              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <span style={{ fontSize: '0.85rem', color: 'var(--text-muted)', display: 'flex', alignItems: 'center', gap: '0.3rem' }}>
                  <Award size={14} /> Completion Rate: {habit.completionRate}%
                </span>

                <button
                  onClick={() => onRecordHabit(habit.id)}
                  className="btn-primary"
                  style={{
                    background: habit.isCompletedToday ? 'var(--success-color)' : 'var(--accent-gradient)',
                    padding: '0.5rem 1rem',
                    fontSize: '0.85rem',
                  }}
                >
                  {habit.isCompletedToday ? '✓ Completed Today' : 'Mark Completed'}
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};
