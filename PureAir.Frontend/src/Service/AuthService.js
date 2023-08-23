import axios from 'axios';

class AuthService {
  async login(email, password) {
    try {
      const response = await axios.post('http://localhost:7031/api/Authentication/LoginAdmin', {
        email,
        password,
      });
      const { token, result, errors } = response.data;
      if (result) {
        localStorage.setItem('token', token);
        return true;
      } else {
        console.error('Login failed:', errors);
        return false;
      }
    } catch (error) {
      console.error('Error during login:', error);
      return false;
    }
  }

  logout() {
    localStorage.removeItem('token');
  }

  isAuthenticated() {
    const token = localStorage.getItem('token');
    return token !== null;
  }
}

export default AuthService;