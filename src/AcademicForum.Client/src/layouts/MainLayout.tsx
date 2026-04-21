import { NavLink, Outlet, useLocation } from "react-router-dom";

const NAV_ITEMS = [
  { to: "/", label: "Dashboard" },
  { to: "/events", label: "Events" },
  { to: "/members", label: "Members" },
  { to: "/movies", label: "Movies" },
  { to: "/venues", label: "Venues" },
];

export default function MainLayout() {
  const location = useLocation();
  const currentPage = NAV_ITEMS.find(
    (item) => item.to !== "/" && location.pathname.startsWith(item.to)
  )?.label ?? "Events";

  return (
    <div className="app-shell">
      <aside className="sidebar">
        <div className="sidebar-header">
          <div className="sidebar-logo">AF</div>
          <span className="sidebar-title">Academic Forum</span>
        </div>
        <nav className="sidebar-nav">
          <p className="nav-section-label">Navigation</p>
          {NAV_ITEMS.map(({ to, label }) => (
            <NavLink
              key={to}
              to={to}
              end={to === "/"}
              className={({ isActive }) => `nav-item ${isActive ? "active" : ""}`}
            >
              {label}
            </NavLink>
          ))}
        </nav>
      </aside>

      <div className="main-wrapper">
        <header className="topbar">
          <span className="topbar-title">{currentPage}</span>
        </header>
        <main className="page-content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}