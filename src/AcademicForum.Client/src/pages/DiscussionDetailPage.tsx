import { useEffect, useState } from "react";
import { useParams, useNavigate, useLocation } from "react-router-dom";
import { discussionsApi, responsesApi } from "../api/services";
import type { Discussion, ApiResponse } from "../types";
import ConfirmModal from "../components/ConfirmModal";

export default function DiscussionDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const [discussion, setDiscussion] = useState<Discussion | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showResponseModal, setShowResponseModal] = useState(false);
  const [responseForm, setResponseForm] = useState({ comment: "" });
  const [saving, setSaving] = useState(false);
  const [confirmDeleteResponse, setConfirmDeleteResponse] = useState<number | null>(null);

  const from = location.state?.from;
  const eventId = location.state?.eventId;
  const movieId = location.state?.movieId;

  const handleBack = () => {
    if (from === "event") navigate(`/events/${eventId}`);
    else if (from === "movie") navigate(`/movies/${movieId}`);
    else navigate("/");
  };

  useEffect(() => { fetchDiscussion(); }, [id]);

  const fetchDiscussion = () => {
    setLoading(true);
    discussionsApi
      .getById(id!)
      .then((res) => {
        const body: ApiResponse<Discussion> = res.data;
        setDiscussion(body.data);
      })
      .catch(() => setError("Failed to load discussion."))
      .finally(() => setLoading(false));
  };

  const handleCreateResponse = () => {
    setSaving(true);
    responsesApi
      .create({
        comment: responseForm.comment,
        date: new Date().toISOString(),
        discussionId: Number(id),
        memberId: 1,
      })
      .then(() => {
        setShowResponseModal(false);
        setResponseForm({ comment: "" });
        fetchDiscussion();
      })
      .catch(() => setError("Failed to create response."))
      .finally(() => setSaving(false));
  };

  const doDeleteResponse = () => {
    if (!confirmDeleteResponse) return;
    responsesApi
      .delete(String(confirmDeleteResponse))
      .then(() => { setConfirmDeleteResponse(null); fetchDiscussion(); })
      .catch(() => setError("Failed to delete response."));
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>{error}</p>;
  if (!discussion) return <p>Discussion not found.</p>;

  return (
    <div>
      <button className="back-btn" onClick={handleBack}>← Back</button>

      {/* Original discussion */}
      <div className="card" style={{ marginTop: 16 }}>
        <p style={{ fontSize: 18, fontWeight: 600, color: "var(--text-primary)" }}>
          {discussion.comment}
        </p>
        <div className="discussion-card-meta" style={{ marginTop: 10 }}>
          <span>{discussion.memberName}</span>
          <span>{new Date(discussion.date).toLocaleString(undefined, {
            month: "short", day: "numeric", year: "numeric",
            hour: "2-digit", minute: "2-digit"
          })}</span>
        </div>
      </div>

      {/* Responses */}
      <div className="section-header" style={{ marginTop: 32 }}>
        <h3>Responses ({discussion.responses?.length ?? 0})</h3>
        <button className="btn btn-primary" onClick={() => setShowResponseModal(true)}>
          + New Response
        </button>
      </div>

      {!discussion.responses || discussion.responses.length === 0 ? (
        <div className="empty-state">No responses yet.</div>
      ) : (
        <div style={{ display: "flex", flexDirection: "column", gap: 10 }}>
          {discussion.responses.map((r) => (
            <div
              key={r.id}
              className="card"
              style={{ marginLeft: 24 }}
            >
              <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start" }}>
                <p style={{ fontSize: 14, color: "var(--text-primary)" }}>{r.comment}</p>
                <button
                  className="btn btn-danger"
                  style={{ padding: "4px 10px", fontSize: 12, marginLeft: 16, flexShrink: 0 }}
                  onClick={() => setConfirmDeleteResponse(r.id)}
                >
                  Delete
                </button>
              </div>
              <div className="discussion-card-meta" style={{ marginTop: 8 }}>
                <span>{r.memberName}</span>
                <span>{new Date(r.date).toLocaleString(undefined, {
                  month: "short", day: "numeric", year: "numeric",
                  hour: "2-digit", minute: "2-digit"
                })}</span>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* New Response Modal */}
      {showResponseModal && (
        <div className="modal-overlay">
          <div className="modal">
            <h3 className="modal-title">New Response</h3>
            <div className="form-group">
              <label className="form-label">Comment</label>
              <textarea
                className="form-textarea"
                value={responseForm.comment}
                onChange={(e) => setResponseForm({ comment: e.target.value })}
                style={{ height: 120 }}
              />
            </div>
            <div className="modal-actions">
              <button className="btn btn-ghost" onClick={() => setShowResponseModal(false)}>Cancel</button>
              <button className="btn btn-primary" onClick={handleCreateResponse} disabled={saving}>
                {saving ? "Saving..." : "Save"}
              </button>
            </div>
          </div>
        </div>
      )}

      {confirmDeleteResponse && (
        <ConfirmModal
          message="Are you sure you want to delete this response?"
          onConfirm={doDeleteResponse}
          onCancel={() => setConfirmDeleteResponse(null)}
        />
      )}
    </div>
  );
}