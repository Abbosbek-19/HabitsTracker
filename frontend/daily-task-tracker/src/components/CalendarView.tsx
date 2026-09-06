import React, { useEffect, useState } from 'react';
import type { CalendarMonth } from '../types';
import { calendarApi } from '../api/client';
import { ChevronLeft, ChevronRight } from 'lucide-react';

export const CalendarView: React.FC = () => {
  const [currentYear, setCurrentYear] = useState(new Date().getFullYear());
  const [currentMonth, setCurrentMonth] = useState(new Date().getMonth() + 1);
  const [calendarData, setCalendarData] = useState<CalendarMonth | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    fetchCalendarData();
  }, [currentYear, currentMonth]);

  const fetchCalendarData = async () => {
    setLoading(true);
    try {
      const data = await calendarApi.getMonthly(currentYear, currentMonth);
      setCalendarData(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handlePrevMonth = () => {
    if (currentMonth === 1) {
      setCurrentMonth(12);
      setCurrentYear((prev) => prev - 1);
    } else {
      setCurrentMonth((prev) => prev - 1);
    }
  };

  const handleNextMonth = () => {
    if (currentMonth === 12) {
      setCurrentMonth(1);
      setCurrentYear((prev) => prev + 1);
    } else {
      setCurrentMonth((prev) => prev + 1);
    }
  };

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1.5rem' }}>
        <div>
          <h1 style={{ fontSize: '1.75rem', fontWeight: 800 }}>Calendar Overview</h1>
          <p style={{ color: 'var(--text-muted)' }}>Visualize daily task workloads and historical check-ins.</p>
        </div>

        <div style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
          <button onClick={handlePrevMonth} className="theme-toggle-btn">
            <ChevronLeft size={18} />
          </button>
          <span style={{ fontWeight: 700, fontSize: '1.1rem' }}>
            {calendarData?.monthName} {currentYear}
          </span>
          <button onClick={handleNextMonth} className="theme-toggle-btn">
            <ChevronRight size={18} />
          </button>
        </div>
      </div>

      <div className="glass-card">
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(7, 1fr)', gap: '0.5rem', marginBottom: '0.75rem', textAlign: 'center', fontWeight: 700, color: 'var(--text-muted)' }}>
          <div>Mon</div><div>Tue</div><div>Wed</div><div>Thu</div><div>Fri</div><div>Sat</div><div>Sun</div>
        </div>

        {loading ? (
          <div style={{ textAlign: 'center', padding: '3rem' }}>Loading calendar data...</div>
        ) : (
          <div style={{ display: 'grid', gridTemplateColumns: 'repeat(7, 1fr)', gap: '0.5rem' }}>
            {calendarData?.days.map((day) => (
              <div
                key={day.date}
                style={{
                  minHeight: '80px',
                  padding: '0.5rem',
                  borderRadius: 'var(--radius-sm)',
                  backgroundColor: day.completedTaskCount > 0 ? 'rgba(16, 185, 129, 0.1)' : 'rgba(255,255,255,0.03)',
                  border: '1px solid var(--border-color)',
                  display: 'flex',
                  flexDirection: 'column',
                  justifyContent: 'space-between',
                }}
              >
                <div style={{ fontWeight: 700, fontSize: '0.85rem' }}>
                  {new Date(day.date).getDate()}
                </div>

                {day.totalTaskCount > 0 && (
                  <div style={{ fontSize: '0.75rem', color: day.completedTaskCount === day.totalTaskCount ? 'var(--success-color)' : 'var(--text-muted)' }}>
                    ✓ {day.completedTaskCount}/{day.totalTaskCount} ({day.completionRate}%)
                  </div>
                )}
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
};
