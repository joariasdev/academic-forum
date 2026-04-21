import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { eventsApi, moviesApi, venuesApi } from "../api/services";
import type { Event, Movie, Venue, ApiResponse } from "../types";
import ConfirmModal from "../components/ConfirmModal";

const emptyForm = {
  title: "",
  description: "",
  date: "",
  time: "",
  movieId: "",
  venueId: "",
};

export default function EventsPage() {
  const [events, setEvents] = useState<Event[]>([]);
  const [movies, setMovies] = useState<Movie[]>([]);
  const [venues, setVenues] = useState<Venue[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [editingEvent, setEditingEvent] = useState<Event | null>(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [confirmDelete, setConfirmDelete] = useState<Event | null>(null);
  const navigate = useNavigate();

  useEffect(() => { fetchEvents(); }, []);

  const fetchEvents = () => {
    setLoading(true);
    eventsApi
      .getAll()
      .then((res) => {
        const body: ApiResponse<Event[]> = res.data;
        setEvents(body.data);
      })
      .catch(() => setError("Failed to load events."))
      .finally(() => setLoading(false));
  };

  const fetchDropdowns = () => {
    Promise.all([moviesApi.getAll(), venuesApi.getAll()]).then(
      ([moviesRes, venuesRes]) => {
        const moviesBody: ApiResponse<Movie[]> = moviesRes.data;
        const venuesBody: ApiResponse<Venue[]> = venuesRes.data;
        setMovies(moviesBody.data);
        setVenues(venuesBody.data);
      }
    );
  };

  const openCreate = () => {
    setEditingEvent(null);
    setForm(emptyForm);
    fetchDropdowns();
    setShowModal(true);
  };

  const openEdit = (event: Event, e: React.MouseEvent) => {
    e.stopPropagation();
    setEditingEvent(event);
    setForm({
      title: event.title,
      description: event.description,
      date: event.date.split("T")[0],
      time: event.date.split("T")[1]?.slice(0, 5) ?? "",
      movieId: String(event.movieId),
      venueId: String(event.venueId),
    });
    fetchDropdowns();
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingEvent(null);
    setForm(emptyForm);
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
  ) => {
    setForm((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = () => {
    setSaving(true);
    const payload = {
      ...form,
      date: `${form.date}T${form.time}:00`,
      movieId: Number(form.movieId),
      venueId: Number(form.venueId),
    };
    const request = editingEvent
      ? eventsApi.update(String(editingEvent.id), payload)
      : eventsApi.create(payload);
    request
      .then(() => { closeModal(); fetchEvents(); })
      .catch(() => setError("Failed to save event."))
      .finally(() => setSaving(false));
  };

  const confirmDeleteEvent = () => {
    if (!confirmDelete) return;
    eventsApi
      .delete(String(confirmDelete.id))
      .then(() => { setConfirmDelete(null); fetchEvents(); })
      .catch(() => setError("Failed to delete event."));
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <div className="page-header">
        <div>
          <h2>Events</h2>
          <p>Manage all academic forum events.</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>+ New Event</button>
      </div>

      <div className="table-container">
        <div className="table-toolbar">
          <span className="table-toolbar-title">All Events</span>
        </div>
        {events.length === 0 ? (
          <div className="empty-state">No events found.</div>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Title</th>
                <th>Date</th>
                <th>Movie</th>
                <th>Venue</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {events.map((event) => (
                <tr
                  key={event.id}
                  onClick={() => navigate(`/events/${event.id}`)}
                  style={{ cursor: "pointer" }}
                >
                  <td>{event.title}</td>
                  <td>{new Date(event.date).toLocaleDateString("en-US", {
                    weekday: "short", year: "numeric", month: "short", day: "numeric"
                  })}</td>
                  <td>{event.movieTitle}</td>
                  <td>{event.venueName}</td>
                  <td onClick={(e) => e.stopPropagation()}>
                    <div style={{ display: "flex", gap: 8 }}>
                      <button className="btn btn-ghost" onClick={(e) => openEdit(event, e)}>Edit</button>
                      <button className="btn btn-danger" onClick={() => setConfirmDelete(event)}>Delete</button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h3 className="modal-title">{editingEvent ? "Edit Event" : "New Event"}</h3>

            <div className="form-group">
              <label className="form-label">Title</label>
              <input className="form-input" name="title" value={form.title} onChange={handleChange} />
            </div>

            <div className="form-group">
              <label className="form-label">Description</label>
              <textarea className="form-textarea" name="description" value={form.description} onChange={handleChange} />
            </div>

            <div className="form-group">
              <label className="form-label">Date</label>
              <input className="form-input" name="date" type="date" value={form.date} onChange={handleChange} />
            </div>

            <div className="form-group">
              <label className="form-label">Time</label>
              <input className="form-input" name="time" type="time" value={form.time} onChange={handleChange}
              />
            </div>

            <div className="form-group">
              <label className="form-label">Movie</label>
              <select className="form-select" name="movieId" value={form.movieId} onChange={handleChange}>
                <option value="">Select a movie...</option>
                {movies.map((m) => (
                  <option key={m.id} value={m.id}>{m.title}</option>
                ))}
              </select>
            </div>

            <div className="form-group">
              <label className="form-label">Venue</label>
              <select className="form-select" name="venueId" value={form.venueId} onChange={handleChange}>
                <option value="">Select a venue...</option>
                {venues.map((v) => (
                  <option key={v.id} value={v.id}>{v.name}</option>
                ))}
              </select>
            </div>

            <div className="modal-actions">
              <button className="btn btn-ghost" onClick={closeModal}>Cancel</button>
              <button className="btn btn-primary" onClick={handleSubmit} disabled={saving}>
                {saving ? "Saving..." : "Save"}
              </button>
            </div>
          </div>
        </div>
      )}

      {confirmDelete && (
        <ConfirmModal
          message={`Are you sure you want to delete "${confirmDelete.title}"?`}
          onConfirm={confirmDeleteEvent}
          onCancel={() => setConfirmDelete(null)}
        />
      )}
    </div>
  );
}