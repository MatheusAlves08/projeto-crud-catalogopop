import React from 'react';
import Sidebar from './Sidebar';
import Navbar from './Navbar';
import styles from './Layout.module.css';

const Layout = ({ children, title }) => {
  return (
    <div className={styles.appContainer}>
      <Sidebar />
      <main className={styles.mainContent}>
        <Navbar title={title} />
        <div className={`${styles.pageContainer} animate-fade-in`}>
          {children}
        </div>
      </main>
    </div>
  );
};

export default Layout;
