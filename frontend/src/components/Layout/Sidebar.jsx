import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom';
import { authService } from '../../services/api';
import styles from './Sidebar.module.css';
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
  const navigate = useNavigate();

  const handleLogout = () => {
    authService.logout();
    navigate('/login');
  };
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
    <aside className={styles.sidebar}>
      <div className={styles.sidebarHeader}>
        <div className={styles.logoContainer}>
          <div className={styles.logoIcon}>
            <Files size={24} color="white" fill="white" />
          </div>
          <h1>Catálogo<span>POP</span></h1>
        </div>
      </div>

      <nav className={styles.sidebarNav}>
        {menuItems.map((item) => (
          <NavLink 
            key={item.path} 
            to={item.path}
            className={({ isActive }) => `${styles.navItem} ${isActive ? styles.navItemActive : ''}`}
          >
            <item.icon size={20} className={styles.navIcon} />
            <span>{item.label}</span>
            {item.label === 'Procedimentos' && <ChevronRight size={14} className={styles.arrow} />}
          </NavLink>
        ))}
      </nav>

      <div className={styles.sidebarFooter}>
        <NavLink to="/ajuda" className={styles.navItem}>
          <HelpCircle size={20} className={styles.navIcon} />
          <span>Ajuda</span>
        </NavLink>
        <button className={styles.logoutBtn} onClick={handleLogout}>
          <LogOut size={20} className={styles.navIcon} />
          <span>Sair</span>
        </button>
      </div>
    </aside>
  );
};

export default Sidebar;
