import { useEffect, useState } from "react";
import { membersApi } from "../api/services";
import type { Member, ApiResponse } from "../types";
import ConfirmModal from "../components/ConfirmModal";

const emptyForm = {
  name: "",
  email: "",
  role: "",
};

export default function MembersPage() {
  const [members, setMembers] = useState<Member[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [editingMember, setEditingMember] = useState<Member | null>(null);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);
  const [confirmDelete, setConfirmDelete] = useState<Member | null>(null);

  useEffect(() => { fetchMembers(); }, []);

  const fetchMembers = () => {
    setLoading(true);
    membersApi
      .getAll()
      .then((res) => {
        const body: ApiResponse<Member[]> = res.data;
        setMembers(body.data.filter((m) => m.role !== "Administrator"));
      })
      .catch(() => setError("Failed to load members."))
      .finally(() => setLoading(false));
  };

  const openCreate = () => {
    setEditingMember(null);
    setForm(emptyForm);
    setShowModal(true);
  };

  const openEdit = (member: Member) => {
    setEditingMember(member);
    setForm({ name: member.name, email: member.email, role: member.role });
    setShowModal(true);
  };

  const closeModal = () => {
    setShowModal(false);
    setEditingMember(null);
    setForm(emptyForm);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = () => {
    setSaving(true);
    const request = editingMember
      ? membersApi.update(String(editingMember.id), form)
      : membersApi.create(form);
    request
      .then(() => { closeModal(); fetchMembers(); })
      .catch(() => setError("Failed to save member."))
      .finally(() => setSaving(false));
  };

  const confirmDeleteMember = () => {
    if (!confirmDelete) return;
    membersApi
      .delete(String(confirmDelete.id))
      .then(() => { setConfirmDelete(null); fetchMembers(); })
      .catch(() => setError("Failed to delete member."));
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div>
      <div className="page-header">
        <div>
          <h2>Members</h2>
          <p>Manage all forum members.</p>
        </div>
        <button className="btn btn-primary" onClick={openCreate}>+ New Member</button>
      </div>

      <div className="table-container">
        <div className="table-toolbar">
          <span className="table-toolbar-title">All Members</span>
        </div>
        {members.length === 0 ? (
          <div className="empty-state">No members found.</div>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Email</th>
                <th>Role</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {members.map((member) => (
                <tr key={member.id}>
                  <td>{member.name}</td>
                  <td>{member.email}</td>
                  <td>{member.role}</td>
                  <td>
                    <div style={{ display: "flex", gap: 8 }}>
                      <button className="btn btn-ghost" onClick={() => openEdit(member)}>Edit</button>
                      <button className="btn btn-danger" onClick={() => setConfirmDelete(member)}>Delete</button>
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
            <h3 className="modal-title">{editingMember ? "Edit Member" : "New Member"}</h3>

            <div className="form-group">
              <label className="form-label">Name</label>
              <input className="form-input" name="name" value={form.name} onChange={handleChange} />
            </div>

            <div className="form-group">
              <label className="form-label">Email</label>
              <input className="form-input" name="email" type="email" value={form.email} onChange={handleChange} />
            </div>

            <div className="form-group">
              <label className="form-label">Role</label>
              <input className="form-input" name="role" value={form.role} onChange={handleChange} />
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
          onConfirm={confirmDeleteMember}
          onCancel={() => setConfirmDelete(null)}
        />
      )}
    </div>
  );
}