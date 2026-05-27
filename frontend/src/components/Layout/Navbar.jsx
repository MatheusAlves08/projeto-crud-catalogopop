import React from 'react';
import { Bell, User, ChevronDown, Search } from 'lucide-react';
import styles from './Navbar.module.css';

const Navbar = ({ title }) => {
  return (
    <header className={`${styles.navbar} glass`}>
      <div>
        <h2 className={styles.pageTitle}>{title}</h2>
      </div>

      <div className={styles.navbarRight}>
        <div className={styles.searchBox}>
          <Search size={18} className={styles.searchIcon} />
          <input type="text" placeholder="Pesquisar..." />
        </div>

        <button className={styles.iconBtn}>
          <Bell size={20} />
          <span className={styles.notificationBadge}>5</span>
        </button>

        <div className={styles.userProfile}>
          <div className={styles.userInfo}>
            <span className={styles.userName}>{localStorage.getItem('username') || 'Administrador'}</span>
            <span className={styles.userRole}>Administrador</span>
          </div>
          <div className={styles.userAvatar}>
            <User size={20} />
          </div>
          <ChevronDown size={16} className={styles.profileArrow} />
        </div>
      </div>
    </header>
  );
};

export default Navbar;
