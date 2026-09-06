import React, { useState } from 'react';
import { HabitFrequency } from '../types';
import { X } from 'lucide-react';

interface CreateHabitModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (habit: {
    name: string;
    description: string;
    frequency: HabitFrequency;
    targetPerPeriod: number;
  }) => Promise<void>;
}

export const CreateHabitModal: React.FC<CreateHabitModalProps> = ({ isOpen, onClose, onSubmit }) => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [frequency, setFrequency] = useState<HabitFrequency>(HabitFrequency.Daily);
  const [targetPerPeriod, setTargetPerPeriod] = useState(1);
  const [submitting, setSubmitting] = useState(false);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim()) return;

    setSubmitting(true);
    try {
      await onSubmit({ name, description, frequency, targetPerPeriod });
      setName('');
      setDescription('');
      onClose();
    } catch (err) {
      console.error(err);
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1.5rem' }}>
          <h2 style={{ fontSize: '1.25rem', fontWeight: 700 }}>🔥 Create New Habit</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', color: 'var(--text-muted)', cursor: 'pointer' }}>
            <X size={20} />
          </button>
        </div>

        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Habit Name *</label>
            <input
              type="text"
              className="form-control"
              placeholder="e.g. Drink 2L Water or Read 20 pages"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
            />
          </div>

          <div className="form-group">
            <label>Description</label>
            <input
              type="text"
              className="form-control"
              placeholder="Motivation or daily goal..."
              value={description}
              onChange={(e) => setDescription(e.target.value)}
            />
          </div>

          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
            <div className="form-group">
              <label>Frequency</label>
              <select
                className="form-control"
                value={frequency}
                onChange={(e) => setFrequency(Number(e.target.value) as HabitFrequency)}
              >
                <option value={HabitFrequency.Daily}>Daily</option>
                <option value={HabitFrequency.Weekly}>Weekly</option>
                <option value={HabitFrequency.Monthly}>Monthly</option>
              </select>
            </div>

            <div className="form-group">
              <label>Target Per Period</label>
              <input
                type="number"
                min="1"
                className="form-control"
                value={targetPerPeriod}
                onChange={(e) => setTargetPerPeriod(Number(e.target.value))}
              />
            </div>
          </div>

          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '0.75rem', marginTop: '1.5rem' }}>
            <button type="button" onClick={onClose} className="btn-primary" style={{ background: 'var(--border-color)', color: 'var(--text-main)', boxShadow: 'none' }}>
              Cancel
            </button>
            <button type="submit" disabled={submitting} className="btn-primary">
              {submitting ? 'Creating...' : 'Create Habit'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
