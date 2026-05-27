import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../../services/api';
import { Lock, User, AlertCircle, Files } from 'lucide-react';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      await authService.login(username, password);
      navigate('/');
    } catch (err) {
      setError(err.message || 'Credenciais inválidas. Dica: use admin / admin123');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-wrapper">
      <div className="login-background">
        <div className="bubble bubble-1"></div>
        <div className="bubble bubble-2"></div>
        <div className="bubble bubble-3"></div>
      </div>
      
      <div className="login-card-container animate-fade-in">
        <div className="login-card glass">
          <div className="login-header">
            <div className="logo-icon-container">
              <Files size={32} color="white" fill="white" />
            </div>
            <h2>HGTX™ CatálogoPOP</h2>
            <p>Faça login para gerenciar os Procedimentos Operacionais Padrão</p>
          </div>

          <form onSubmit={handleSubmit} className="login-form">
            {error && (
              <div className="error-alert">
                <AlertCircle size={18} />
                <span>{error}</span>
              </div>
            )}

            <div className="input-group">
              <label htmlFor="username">Usuário</label>
              <div className="input-field-wrapper">
                <User size={18} className="input-icon" />
                <input
                  id="username"
                  type="text"
                  placeholder="Nome de usuário (ex: admin)"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  required
                  autoComplete="username"
                />
              </div>
            </div>

            <div className="input-group">
              <label htmlFor="password">Senha</label>
              <div className="input-field-wrapper">
                <Lock size={18} className="input-icon" />
                <input
                  id="password"
                  type="password"
                  placeholder="Sua senha (ex: admin123)"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                  autoComplete="current-password"
                />
              </div>
            </div>

            <button type="submit" className="btn btn-primary login-btn" disabled={loading}>
              {loading ? 'Entrando...' : 'Entrar na Plataforma'}
            </button>
          </form>
        </div>
      </div>

      <style dangerouslySetInnerHTML={{ __html: `
        .login-wrapper {
          position: relative;
          min-height: 100vh;
          display: flex;
          align-items: center;
          justify-content: center;
          background: #0f172a; /* Deep elegant dark background */
          overflow: hidden;
          font-family: 'Inter', system-ui, sans-serif;
          padding: 1.5rem;
        }

        .login-background {
          position: absolute;
          width: 100%;
          height: 100%;
          top: 0;
          left: 0;
          z-index: 1;
        }

        .bubble {
          position: absolute;
          border-radius: 50%;
          filter: blur(80px);
          opacity: 0.15;
          animation: float 20s infinite alternate;
        }

        .bubble-1 {
          top: -10%;
          left: -10%;
          width: 500px;
          height: 500px;
          background: var(--primary, #2563eb);
        }

        .bubble-2 {
          bottom: -10%;
          right: -10%;
          width: 600px;
          height: 600px;
          background: #3b82f6;
          animation-delay: -5s;
        }

        .bubble-3 {
          top: 40%;
          left: 50%;
          width: 300px;
          height: 300px;
          background: #8b5cf6;
          animation-delay: -10s;
        }

        @keyframes float {
          0% { transform: translate(0, 0) scale(1); }
          100% { transform: translate(50px, 50px) scale(1.1); }
        }

        .login-card-container {
          position: relative;
          z-index: 2;
          width: 100%;
          max-width: 460px;
        }

        .login-card {
          background: rgba(30, 41, 59, 0.7); /* Deep slate with opacity */
          border: 1px solid rgba(255, 255, 255, 0.08);
          border-radius: 20px;
          padding: 3rem 2.5rem;
          box-shadow: 0 20px 40px rgba(0, 0, 0, 0.4);
          backdrop-filter: blur(16px);
        }

        .login-header {
          text-align: center;
          margin-bottom: 2.5rem;
        }

        .logo-icon-container {
          width: 60px;
          height: 60px;
          background: linear-gradient(135deg, var(--primary, #2563eb), #3b82f6);
          border-radius: 14px;
          display: flex;
          align-items: center;
          justify-content: center;
          margin: 0 auto 1.5rem;
          box-shadow: 0 8px 24px rgba(37, 99, 235, 0.35);
        }

        .login-header h2 {
          color: white;
          font-size: 1.75rem;
          font-weight: 700;
          margin-bottom: 0.5rem;
          letter-spacing: -0.5px;
        }

        .login-header p {
          color: #94a3b8;
          font-size: 0.95rem;
          line-height: 1.5;
        }

        .login-form {
          display: flex;
          flex-direction: column;
          gap: 1.5rem;
        }

        .error-alert {
          display: flex;
          align-items: center;
          gap: 10px;
          background: rgba(239, 68, 68, 0.15);
          border: 1px solid rgba(239, 68, 68, 0.3);
          color: #fca5a5;
          padding: 0.75rem 1rem;
          border-radius: 8px;
          font-size: 0.875rem;
        }

        .input-group {
          display: flex;
          flex-direction: column;
          gap: 8px;
        }

        .input-group label {
          color: #cbd5e1;
          font-size: 0.875rem;
          font-weight: 500;
        }

        .input-field-wrapper {
          position: relative;
          display: flex;
          align-items: center;
        }

        .input-icon {
          position: absolute;
          left: 14px;
          color: #64748b;
          pointer-events: none;
        }

        .input-field-wrapper input {
          width: 100%;
          background: rgba(15, 23, 42, 0.6);
          border: 1px solid rgba(255, 255, 255, 0.1);
          color: white;
          padding: 0.75rem 1rem 0.75rem 2.75rem;
          border-radius: 10px;
          font-size: 0.95rem;
          outline: none;
          transition: all 0.2s ease-in-out;
        }

        .input-field-wrapper input:focus {
          border-color: var(--primary, #2563eb);
          box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.2);
          background: rgba(15, 23, 42, 0.8);
        }

        .login-btn {
          margin-top: 1rem;
          padding: 0.875rem;
          font-size: 1rem;
          font-weight: 600;
          border-radius: 10px;
          border: none;
          background: linear-gradient(135deg, var(--primary, #2563eb), #1d4ed8);
          color: white;
          cursor: pointer;
          transition: all 0.2s ease-in-out;
          box-shadow: 0 4px 12px rgba(37, 99, 235, 0.25);
        }

        .login-btn:hover:not(:disabled) {
          transform: translateY(-1px);
          box-shadow: 0 6px 16px rgba(37, 99, 235, 0.4);
        }

        .login-btn:disabled {
          opacity: 0.6;
          cursor: not-allowed;
        }
      `}} />
    </div>
  );
};

export default Login;
