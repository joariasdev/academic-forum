import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { eventsApi, membersApi, attendeeRecordsApi, venuesApi } from "../api/services";
import type { Event, Member, AttendeeRecord, Venue, ApiResponse } from "../types";
import ConfirmModal from "../components/ConfirmModal";

export default function AttendancePage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [event, setEvent] = useState<Event | null>(null);
  const [venue, setVenue] = useState<Venue | null>(null);
  const [allMembers, setAllMembers] = useState<Member[]>([]);
  const [records, setRecords] = useState<AttendeeRecord[]>([]);
  const [selectedMemberId, setSelectedMemberId] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [saving, setSaving] = useState(false);
  const [confirmRemove, setConfirmRemove] = useState<AttendeeRecord | null>(null);

  useEffect(() => { fetchData(); }, [id]);

  const fetchData = () => {
    setLoading(true);
    Promise.all([eventsApi.getById(id!), membersApi.getAll()])
      .then(([eventRes, membersRes]) => {
        const eventBody: ApiResponse<Event> = eventRes.data;
        const membersBody: ApiResponse<Member[]> = membersRes.data;
        const eventData = eventBody.data;
        setEvent(eventData);
        setRecords(eventData.attendeeRecords ?? []);
        setAllMembers(membersBody.data.filter((m) => m.role !== "Administrator"));
        return venuesApi.getById(String(eventData.venueId));
      })
      .then((venueRes) => {
        const venueBody: ApiResponse<Venue> = venueRes.data;
        setVenue(venueBody.data);
      })
      .catch(() => setError("Failed to load data."))
      .finally(() => setLoading(false));
  };

  const unregisteredMembers = allMembers.filter(
    (m) => !records.some((r) => r.memberId === m.id)
  );

  const handleToggleAttendance = (record: AttendeeRecord) => {
    attendeeRecordsApi
      .update(String(record.id), { ...record, hasAttended: !record.hasAttended })
      .then(() => fetchData())
      .catch(() => setError("Failed to update attendance."));
  };

  const handleRegister = () => {
    if (!selectedMemberId) return;
    setSaving(true);
    attendeeRecordsApi
      .create({
        eventId: Number(id),
        memberId: Number(selectedMemberId),
        hasAttended: false,
      })
      .then(() => { setSelectedMemberId(""); fetchData(); })
      .catch(() => setError("Failed to register member."))
      .finally(() => setSaving(false));
  };

  const doRemove = () => {
    if (!confirmRemove) return;
    attendeeRecordsApi
      .delete(String(confirmRemove.id))
      .then(() => { setConfirmRemove(null); fetchData(); })
      .catch(() => setError("Failed to remove member."));
  };

  const capacityStatus = () => {
    if (!venue) return null;
    const pct = records.length / venue.capacity;
    if (pct >= 1) return "red";
    if (pct >= 0.9) return "orange";
    return "green";
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <button className="back-btn" onClick={() => navigate(`/events/${id}`)}>
        ← Back to Event
      </button>

      <h1 className="detail-title">Manage Attendance</h1>
      <p style={{ fontSize: 15, color: "var(--text-secondary)", marginBottom: 8 }}>
        {event?.title}
      </p>

      {venue && (
        <p className={`attendance-status ${capacityStatus()}`}>
          Registered: {records.length} / {venue.capacity} capacity
          {records.length >= venue.capacity && " — Event is full"}
          {records.length >= venue.capacity * 0.9 && records.length < venue.capacity && " — Almost full"}
        </p>
      )}

      <hr className="detail-divider" />

      {/* Register new member */}
      <div className="section-header">
        <h3>Register Member</h3>
      </div>

      {unregisteredMembers.length === 0 ? (
        <p style={{ fontSize: 14, color: "var(--text-muted)" }}>All members are already registered.</p>
      ) : (
        <div style={{ display: "flex", gap: 10, alignItems: "center" }}>
          <select
            className="form-select"
            style={{ width: 280 }}
            value={selectedMemberId}
            onChange={(e) => setSelectedMemberId(e.target.value)}
          >
            <option value="">Select a member...</option>
            {unregisteredMembers.map((m) => (
              <option key={m.id} value={m.id}>{m.name}</option>
            ))}
          </select>
          <button
            className="btn btn-primary"
            onClick={handleRegister}
            disabled={saving || !selectedMemberId}
          >
            {saving ? "Registering..." : "Register"}
          </button>
        </div>
      )}

      <hr className="detail-divider" />

      {/* Registered members */}
      <div className="section-header">
        <h3>Registered Members</h3>
      </div>

      {records.length === 0 ? (
        <div className="empty-state">No members registered yet.</div>
      ) : (
        <div className="table-container">
          <table>
            <thead>
              <tr>
                <th>Member</th>
                <th>Attended</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {records.map((record) => (
                <tr key={record.id}>
                  <td>{record.memberName}</td>
                  <td>
                    <input
                      type="checkbox"
                      checked={record.hasAttended}
                      onChange={() => handleToggleAttendance(record)}
                    />
                  </td>
                  <td>
                    <button
                      className="btn btn-danger"
                      style={{ padding: "4px 10px", fontSize: 12 }}
                      onClick={() => setConfirmRemove(record)}
                    >
                      Remove
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {confirmRemove && (
        <ConfirmModal
          message={`Are you sure you want to remove "${confirmRemove.memberName}" from this event?`}
          onConfirm={doRemove}
          onCancel={() => setConfirmRemove(null)}
        />
      )}
    </div>
  );
}