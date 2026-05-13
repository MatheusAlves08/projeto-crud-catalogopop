import React from 'react';
import { Bell, User, ChevronDown, Search } from 'lucide-react';

const Navbar = ({ title }) => {
  return (
    <header className="navbar glass">
      <div className="navbar-left">
        <h2 className="page-title">{title}</h2>
      </div>

      <div className="navbar-right">
        <div className="search-box">
          <Search size={18} className="search-icon" />
          <input type="text" placeholder="Pesquisar..." />
        </div>

        <button className="icon-btn">
          <Bell size={20} />
          <span className="notification-badge">5</span>
        </button>

        <div className="user-profile">
          <div className="user-info">
            <span className="user-name">João Silva</span>
            <span className="user-role">Administrador</span>
          </div>
          <div className="user-avatar">
            <User size={20} />
          </div>
          <ChevronDown size={16} className="profile-arrow" />
        </div>
      </div>

      <style dangerouslySetInnerHTML={{ __html: `
        .navbar {
          height: 70px;
          display: flex;
          align-items: center;
          justify-content: space-between;
          padding: 0 2rem;
          position: sticky;
          top: 0;
          z-index: 90;
          margin-bottom: 2rem;
          border-bottom: 1px solid var(--border);
        }

        .page-title {
          font-size: 1.5rem;
          font-weight: 600;
          color: var(--text-main);
        }

        .navbar-right {
          display: flex;
          align-items: center;
          gap: 1.5rem;
        }

        .search-box {
          position: relative;
          display: flex;
          align-items: center;
          background: rgba(0, 0, 0, 0.03);
          padding: 0.5rem 1rem;
          border-radius: 20px;
          width: 240px;
          transition: var(--transition);
        }

        .search-box:focus-within {
          background: white;
          box-shadow: 0 0 0 2px rgba(37, 99, 235, 0.2);
          width: 300px;
        }

        .search-icon {
          color: var(--text-muted);
          margin-right: 8px;
        }

        .search-box input {
          background: transparent;
          border: none;
          outline: none;
          width: 100%;
          font-size: 0.9rem;
        }

        .icon-btn {
          position: relative;
          background: transparent;
          color: var(--text-muted);
          display: flex;
          align-items: center;
          justify-content: center;
          width: 40px;
          height: 40px;
          border-radius: 50%;
        }

        .icon-btn:hover {
          background: rgba(0, 0, 0, 0.05);
          color: var(--primary);
        }

        .notification-badge {
          position: absolute;
          top: 4px;
          right: 4px;
          background: var(--danger);
          color: white;
          font-size: 10px;
          font-weight: 700;
          width: 16px;
          height: 16px;
          border-radius: 50%;
          display: flex;
          align-items: center;
          justify-content: center;
          border: 2px solid white;
        }

        .user-profile {
          display: flex;
          align-items: center;
          gap: 12px;
          padding: 6px;
          border-radius: 30px;
          transition: var(--transition);
          cursor: pointer;
        }

        .user-profile:hover {
          background: rgba(0, 0, 0, 0.03);
        }

        .user-info {
          display: flex;
          flex-direction: column;
          align-items: flex-end;
        }

        .user-name {
          font-size: 0.9rem;
          font-weight: 600;
        }

        .user-role {
          font-size: 0.75rem;
          color: var(--text-muted);
        }

        .user-avatar {
          width: 40px;
          height: 40px;
          background: var(--primary);
          color: white;
          border-radius: 50%;
          display: flex;
          align-items: center;
          justify-content: center;
          box-shadow: 0 4px 10px rgba(37, 99, 235, 0.2);
        }

        .profile-arrow {
          color: var(--text-muted);
        }
      `}} />
    </header>
  );
};

export default Navbar;
