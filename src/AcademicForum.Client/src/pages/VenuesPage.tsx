import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { venuesApi } from "../api/services";
import type { Venue, ApiResponse } from "../types";
import ConfirmModal from "../components/ConfirmModal";

const emptyForm = {
  name: "",
  address: "",
  capacity: "",
};

export default function VenuesPage() {
  const [venues, setVenues] = useState<Venue[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [editingVenue, setEditingVenue] = useState<Venue | null>(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [confirmDelete, setConfirmDelete] = useState<Venue | null>(null);
  const navigate = useNavigate();

  useEffect(() => { fetchVenues(); }, []);

  const fetchVenues = () => {
    setLoading(true);
    venuesApi
      .getAll()
      .then((res) => {
        const body: ApiResponse<Venue[]> = res.data;
        setVenues(body.data);
      })
      .catch(() => setError("Failed to load venues."))
      .finally(() => setLoading(false));
  };

  const openCreate = () => {
    setEditingVenue(null);
    setForm(emptyForm);
    setShowModal(true);
  };

  const openEdit = (venue: Venue, e: React.MouseEvent) => {
    e.stopPropagation();
    setEditingVenue(venue);
    setForm({
      name: venue.name,
      address: venue.address,
      capacity: String(venue.capacity),
    });
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingVenue(null);
    setForm(emptyForm);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = () => {
    setSaving(true);
    const payload = { ...form, capacity: Number(form.capacity) };
    const request = editingVenue
      ? venuesApi.update(String(editingVenue.id), payload)
      : venuesApi.create(payload);
    request
      .then(() => { closeModal(); fetchVenues(); })
      .catch(() => setError("Failed to save venue."))
      .finally(() => setSaving(false));
  };

  const confirmDeleteVenue = () => {
    if (!confirmDelete) return;
    venuesApi
      .delete(String(confirmDelete.id))
      .then(() => { setConfirmDelete(null); fetchVenues(); })
      .catch(() => setError("Failed to delete venue."));
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <div className="page-header">
        <div>
          <h2>Venues</h2>
          <p>Manage all event venues and their capacity.</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>+ New Venue</button>
      </div>

      <div className="table-container">
        <div className="table-toolbar">
          <span className="table-toolbar-title">All Venues</span>
        </div>
        {venues.length === 0 ? (
          <div className="empty-state">No venues found.</div>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Address</th>
                <th>Capacity</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {venues.map((venue) => (
                <tr
                  key={venue.id}
                  onClick={() => navigate(`/venues/${venue.id}`)}
                  style={{ cursor: "pointer" }}
                >
                  <td>{venue.name}</td>
                  <td>{venue.address}</td>
                  <td>{venue.capacity}</td>
                  <td onClick={(e) => e.stopPropagation()}>
                    <div style={{ display: "flex", gap: 8 }}>
                      <button className="btn btn-ghost" onClick={(e) => openEdit(venue, e)}>Edit</button>
                      <button className="btn btn-danger" onClick={() => setConfirmDelete(venue)}>Delete</button>
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
            <h3 className="modal-title">{editingVenue ? "Edit Venue" : "New Venue"}</h3>

            <div className="form-group">
              <label className="form-label">Name</label>
              <input className="form-input" name="name" value={form.name} onChange={handleChange} />
            </div>

            <div className="form-group">
              <label className="form-label">Address</label>
              <input className="form-input" name="address" value={form.address} onChange={handleChange} />
            </div>

            <div className="form-group">
              <label className="form-label">Capacity</label>
              <input className="form-input" name="capacity" type="number" value={form.capacity} onChange={handleChange} />
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
          message={`Are you sure you want to delete "${confirmDelete.name}"?`}
          onConfirm={confirmDeleteVenue}
          onCancel={() => setConfirmDelete(null)}
        />
      )}
    </div>
  );
}