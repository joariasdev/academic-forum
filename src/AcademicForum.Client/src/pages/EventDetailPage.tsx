import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { eventsApi, discussionsApi } from "../api/services";
import type { Event, ApiResponse } from "../types";
import ConfirmModal from "../components/ConfirmModal";

export default function EventDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [event, setEvent] = useState<Event | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showDiscussionModal, setShowDiscussionModal] = useState(false);
  const [discussionForm, setDiscussionForm] = useState({ comment: "" });
  const [saving, setSaving] = useState(false);
  const [confirmDeleteEvent, setConfirmDeleteEvent] = useState(false);
  const [confirmDeleteDiscussion, setConfirmDeleteDiscussion] = useState<number | null>(null);

  useEffect(() => { fetchEvent(); }, [id]);

  const fetchEvent = () => {
    setLoading(true);
    eventsApi
      .getById(id!)
      .then((res) => {
        const body: ApiResponse<Event> = res.data;
        setEvent(body.data);
      })
      .catch(() => setError("Failed to load event."))
      .finally(() => setLoading(false));
  };

  const doDeleteEvent = () => {
    eventsApi
      .delete(id!)
      .then(() => navigate("/"))
      .catch(() => setError("Failed to delete event."));
  };

  const doDeleteDiscussion = () => {
    if (!confirmDeleteDiscussion) return;
    discussionsApi
      .delete(String(confirmDeleteDiscussion))
      .then(() => { setConfirmDeleteDiscussion(null); fetchEvent(); })
      .catch(() => setError("Failed to delete discussion."));
  };

  const handleCreateDiscussion = () => {
    setSaving(true);
    discussionsApi
      .create({
        comment: discussionForm.comment,
        date: new Date().toISOString(),
        eventId: Number(id),
        movieId: event?.movieId,
        memberId: 1,
      })
      .then(() => {
        setShowDiscussionModal(false);
        setDiscussionForm({ comment: "" });
        fetchEvent();
      })
      .catch(() => setError("Failed to create discussion."))
      .finally(() => setSaving(false));
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;
  if (!event) return <p>Event not found.</p>;

  return (
    <div>
      <div className="detail-header">
        <button className="back-btn" onClick={() => navigate("/")}>← Back to Events</button>
        <div style={{ display: "flex", gap: 8 }}>
          <button className="btn btn-accent" onClick={() => navigate(`/events/${id}/attendance`)}>
            Manage Attendance
          </button>
          <button className="btn btn-danger" onClick={() => setConfirmDeleteEvent(true)}>
            Delete Event
          </button>
        </div>
      </div>

      <h1 className="detail-title">{event.title}</h1>

      <div className="detail-meta">
        <p><strong>Description:</strong> {event.description}</p>
        <p><strong>Date:</strong> {new Date(event.date).toLocaleDateString(undefined, {
          weekday: "long", year: "numeric", month: "long", day: "numeric", hour: "2-digit", minute: "2-digit"
        })}</p>
        <p><strong>Movie:</strong> {event.movieTitle}</p>
        <p><strong>Venue:</strong> {event.venueName}</p>
      </div>

      <hr className="detail-divider" />

      <div className="section-header">
        <h3>Discussions</h3>
        <button className="btn btn-primary" onClick={() => setShowDiscussionModal(true)}>
          + New Discussion
        </button>
      </div>

      {!event.discussions || event.discussions.length === 0 ? (
        <div className="empty-state">No discussions yet.</div>
      ) : (
        <div style={{ display: "flex", flexDirection: "column", gap: 12 }}>
          {event.discussions.map((d) => (
            <div key={d.id} className="discussion-card">
              <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start" }}>
                <p
                  className="discussion-card-title"
                  onClick={() => navigate(`/discussions/${d.id}`, { state: { from: "event", eventId: id } })}
                >
                  {d.comment}
                </p>
                <button
                  className="btn btn-danger"
                  style={{ padding: "4px 10px", fontSize: 12 }}
                  onClick={(e) => { e.stopPropagation(); setConfirmDeleteDiscussion(d.id); }}
                >
                  Delete
                </button>
              </div>
              <div
                className="discussion-card-meta"
                onClick={() => navigate(`/discussions/${d.id}`, { state: { from: "event", eventId: id } })}
              >
                <span>{d.memberName}</span>
                <span>{new Date(d.date).toLocaleString(undefined, {
                  month: "short", day: "numeric", year: "numeric",
                  hour: "2-digit", minute: "2-digit"
                })}</span>
                <span>{d.responseCount ?? 0} responses</span>
              </div>
            </div>
          ))}
        </div>
      )}

      {showDiscussionModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h3 className="modal-title">New Discussion</h3>
            <div className="form-group">
              <label className="form-label">Comment</label>
              <textarea
                className="form-textarea"
                value={discussionForm.comment}
                onChange={(e) => setDiscussionForm({ comment: e.target.value })}
                style={{ height: 120 }}
              />
            </div>
            <div className="modal-actions">
              <button className="btn btn-ghost" onClick={() => setShowDiscussionModal(false)}>Cancel</button>
              <button className="btn btn-primary" onClick={handleCreateDiscussion} disabled={saving}>
                {saving ? "Saving..." : "Save"}
              </button>
            </div>
          </div>
        </div>
      )}

      {confirmDeleteEvent && (
        <ConfirmModal
          message={`Are you sure you want to delete "${event.title}"?`}
          onConfirm={doDeleteEvent}
          onCancel={() => setConfirmDeleteEvent(false)}
        />
      )}

      {confirmDeleteDiscussion && (
        <ConfirmModal
          message="Are you sure you want to delete this discussion?"
          onConfirm={doDeleteDiscussion}
          onCancel={() => setConfirmDeleteDiscussion(null)}
        />
      )}
    </div>
  );
}