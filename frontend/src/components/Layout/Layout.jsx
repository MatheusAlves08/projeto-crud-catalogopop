import React from 'react';
import Sidebar from './Sidebar';
import Navbar from './Navbar';

const Layout = ({ children, title }) => {
  return (
    <div className="app-container">
      <Sidebar />
      <main className="main-content">
        <Navbar title={title} />
        <div className="page-container animate-fade-in">
          {children}
        </div>
      </main>

      <style dangerouslySetInnerHTML={{ __html: `
        .app-container {
          display: flex;
          min-height: 100vh;
        }

        .main-content {
          flex: 1;
          margin-left: var(--sidebar-width);
          padding-bottom: 2rem;
          background-color: var(--bg-main);
        }

        .page-container {
          padding: 0 2rem;
          max-width: 1400px;
          margin: 0 auto;
        }
      `}} />
    </div>
  );
};

export default Layout;
