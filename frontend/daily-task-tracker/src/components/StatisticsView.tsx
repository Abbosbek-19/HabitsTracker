import React, { useEffect, useState } from 'react';
import type { StatisticsOverview } from '../types';
import { statisticsApi } from '../api/client';
import { BarChart2, CheckCircle, Clock, Flame, TrendingUp } from 'lucide-react';

export const StatisticsView: React.FC = () => {
  const [stats, setStats] = useState<StatisticsOverview | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchStats();
  }, []);

  const fetchStats = async () => {
    try {
      const data = await statisticsApi.getOverview();
      setStats(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div>Loading analytics overview...</div>;

  return (
    <div>
      <div style={{ marginBottom: '1.5rem' }}>
        <h1 style={{ fontSize: '1.75rem', fontWeight: 800 }}>Analytics & Statistics</h1>
        <p style={{ color: 'var(--text-muted)' }}>Analyze productivity completion rates and weekly workload distribution.</p>
      </div>

      <div className="grid-dashboard" style={{ gridTemplateColumns: 'repeat(auto-fit, minmax(220px, 1fr))' }}>
        <div className="glass-card" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <div style={{ width: '48px', height: '48px', borderRadius: '12px', background: 'rgba(99,102,241,0.15)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <BarChart2 color="#6366f1" size={24} />
          </div>
          <div>
            <div style={{ fontSize: '1.5rem', fontWeight: 800 }}>{stats?.totalTasks}</div>
            <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>Total Tasks</div>
          </div>
        </div>

        <div className="glass-card" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <div style={{ width: '48px', height: '48px', borderRadius: '12px', background: 'rgba(16,185,129,0.15)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <CheckCircle color="#10b981" size={24} />
          </div>
          <div>
            <div style={{ fontSize: '1.5rem', fontWeight: 800 }}>{stats?.completedTasks}</div>
            <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>Completed ({stats?.completionRate}%)</div>
          </div>
        </div>

        <div className="glass-card" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <div style={{ width: '48px', height: '48px', borderRadius: '12px', background: 'rgba(245,158,11,0.15)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <Clock color="#f59e0b" size={24} />
          </div>
          <div>
            <div style={{ fontSize: '1.5rem', fontWeight: 800 }}>{stats?.incompleteTasks}</div>
            <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>Incomplete Tasks</div>
          </div>
        </div>

        <div className="glass-card" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <div style={{ width: '48px', height: '48px', borderRadius: '12px', background: 'rgba(239,68,68,0.15)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <Flame color="#ef4444" size={24} />
          </div>
          <div>
            <div style={{ fontSize: '1.5rem', fontWeight: 800 }}>{stats?.activeHabitStreaks}</div>
            <div style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>Active Habits</div>
          </div>
        </div>
      </div>

      <div className="glass-card" style={{ marginTop: '1.5rem' }}>
        <h3 style={{ fontSize: '1.2rem', fontWeight: 700, marginBottom: '1.5rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
          <TrendingUp color="var(--accent-primary)" size={20} /> Weekly Productivity Breakdown
        </h3>

        <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          {stats?.weeklyBreakdown.map((day) => (
            <div key={day.dayOfWeek} style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
              <div style={{ width: '100px', fontWeight: 600, fontSize: '0.9rem' }}>{day.dayOfWeek}</div>
              <div style={{ flex: 1 }}>
                <div className="progress-bar-container" style={{ margin: 0 }}>
                  <div className="progress-bar-fill" style={{ width: `${day.completionRate}%` }}></div>
                </div>
              </div>
              <div style={{ width: '80px', textAlign: 'right', fontWeight: 700, fontSize: '0.85rem' }}>
                {day.completionRate}%
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};
