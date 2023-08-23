import AuthService from '../Service/AuthService';
import { Outlet, Navigate } from 'react-router-dom';
const authService = new AuthService();

const ProtectedRoute = () => {
  const isAuthenticated = authService.isAuthenticated();
  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />

};

export default ProtectedRoute;