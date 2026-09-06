import React from 'react';
import { LayoutDashboard, CheckSquare, Flame, Calendar as CalendarIcon, BarChart2, Zap } from 'lucide-react';

interface SidebarProps {
  currentTab: string;
  onSelectTab: (tab: string) => void;
}

export const Sidebar: React.FC<SidebarProps> = ({ currentTab, onSelectTab }) => {
  const menuItems = [
    { id: 'dashboard', label: 'Dashboard', icon: LayoutDashboard },
    { id: 'tasks', label: 'Tasks', icon: CheckSquare },
    { id: 'habits', label: 'Habits', icon: Flame },
    { id: 'calendar', label: 'Calendar', icon: CalendarIcon },
    { id: 'statistics', label: 'Statistics', icon: BarChart2 },
  ];

  return (
    <aside className="sidebar">
      <div>
        <div className="logo-container">
          <div className="logo-badge">⚡</div>
          <div>
            <div className="logo-text">TaskTracker</div>
            <span style={{ fontSize: '0.7rem', color: '#94a3b8' }}>Portfolio Pro Edition</span>
          </div>
        </div>

        <ul className="nav-list">
          {menuItems.map((item) => {
            const Icon = item.icon;
            const isActive = currentTab === item.id;
            return (
              <li
                key={item.id}
                className={`nav-item ${isActive ? 'active' : ''}`}
                onClick={() => onSelectTab(item.id)}
              >
                <Icon size={18} />
                <span>{item.label}</span>
              </li>
            );
          })}
        </ul>
      </div>

      <div className="glass-card" style={{ padding: '1rem', background: 'rgba(255,255,255,0.05)' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', marginBottom: '0.5rem' }}>
          <Zap size={16} color="#f59e0b" />
          <span style={{ fontSize: '0.8rem', fontWeight: 600, color: '#ffffff' }}>C# .NET Backend</span>
        </div>
        <p style={{ fontSize: '0.75rem', color: '#94a3b8', lineHeight: 1.4 }}>
          Connected to ASP.NET Core REST API & EF Core PostgreSQL.
        </p>
      </div>
    </aside>
  );
};
