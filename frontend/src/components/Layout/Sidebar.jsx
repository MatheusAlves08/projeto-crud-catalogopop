import React from 'react';
import { NavLink } from 'react-router-dom';
import { 
  LayoutDashboard, 
  FileText, 
  Tags, 
  Building2, 
  Users, 
  History, 
  Files, 
  BarChart3, 
  Settings, 
  HelpCircle,
  ChevronRight,
  LogOut
} from 'lucide-react';

const Sidebar = () => {
  const menuItems = [
    { icon: LayoutDashboard, label: 'Dashboard', path: '/' },
    { icon: FileText, label: 'Procedimentos', path: '/procedimentos' },
    { icon: Tags, label: 'Categorias', path: '/categorias' },
    { icon: Building2, label: 'Departamentos', path: '/departamentos' },
    { icon: Users, label: 'Responsáveis', path: '/responsaveis' },
    { icon: History, label: 'Revisões', path: '/revisoes' },
    { icon: Files, label: 'Documentos', path: '/documentos' },
    { icon: BarChart3, label: 'Relatórios', path: '/relatorios' },
    { icon: Settings, label: 'Configurações', path: '/configuracoes' },
  ];

  return (
    <aside className="sidebar">
      <div className="sidebar-header">
        <div className="logo-container">
          <div className="logo-icon">
            <Files size={24} color="white" fill="white" />
          </div>
          <h1>Catálogo<span>POP</span></h1>
        </div>
      </div>

      <nav className="sidebar-nav">
        {menuItems.map((item) => (
          <NavLink 
            key={item.path} 
            to={item.path}
            className={({ isActive }) => `nav-item ${isActive ? 'active' : ''}`}
          >
            <item.icon size={20} className="nav-icon" />
            <span>{item.label}</span>
            {item.label === 'Procedimentos' && <ChevronRight size={14} className="arrow" />}
          </NavLink>
        ))}
      </nav>

      <div className="sidebar-footer">
        <NavLink to="/ajuda" className="nav-item">
          <HelpCircle size={20} className="nav-icon" />
          <span>Ajuda</span>
        </NavLink>
        <button className="logout-btn">
          <LogOut size={20} className="nav-icon" />
          <span>Sair</span>
        </button>
      </div>

      <style dangerouslySetInnerHTML={{ __html: `
        .sidebar {
          width: var(--sidebar-width);
          height: 100vh;
          background-color: var(--bg-sidebar);
          color: white;
          display: flex;
          flex-direction: column;
          position: fixed;
          left: 0;
          top: 0;
          z-index: 100;
          padding: 1.5rem 0;
        }

        .sidebar-header {
          padding: 0 1.5rem 2rem;
        }

        .logo-container {
          display: flex;
          align-items: center;
          gap: 12px;
        }

        .logo-icon {
          width: 36px;
          height: 36px;
          background: var(--primary);
          border-radius: 8px;
          display: flex;
          align-items: center;
          justify-content: center;
          box-shadow: 0 4px 12px rgba(37, 99, 235, 0.3);
        }

        .logo-container h1 {
          font-size: 1.25rem;
          font-weight: 700;
          letter-spacing: -0.5px;
        }

        .logo-container h1 span {
          color: var(--primary);
        }

        .sidebar-nav {
          flex: 1;
          display: flex;
          flex-direction: column;
          gap: 4px;
          padding: 0 0.75rem;
        }

        .nav-item {
          display: flex;
          align-items: center;
          gap: 12px;
          padding: 0.75rem 1rem;
          border-radius: 8px;
          color: #94a3b8;
          font-weight: 500;
          font-size: 0.95rem;
          transition: var(--transition);
        }

        .nav-item:hover {
          color: white;
          background: rgba(255, 255, 255, 0.05);
        }

        .nav-item.active {
          color: white;
          background: rgba(37, 99, 235, 0.15);
          border-left: 3px solid var(--primary);
          border-radius: 4px 8px 8px 4px;
        }

        .nav-icon {
          flex-shrink: 0;
        }

        .arrow {
          margin-left: auto;
          opacity: 0.5;
        }

        .sidebar-footer {
          margin-top: auto;
          padding: 1rem 0.75rem 0;
          border-top: 1px solid rgba(255, 255, 255, 0.1);
          display: flex;
          flex-direction: column;
          gap: 4px;
        }

        .logout-btn {
          background: transparent;
          width: 100%;
          text-align: left;
          display: flex;
          align-items: center;
          gap: 12px;
          padding: 0.75rem 1rem;
          border-radius: 8px;
          color: #94a3b8;
          font-weight: 500;
        }

        .logout-btn:hover {
          color: var(--danger);
          background: rgba(239, 68, 68, 0.05);
        }
      `}} />
    </aside>
  );
};

export default Sidebar;
