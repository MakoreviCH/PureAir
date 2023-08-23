import { useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import {
  Container,
  Typography,
  TextField,
  Button,
} from '@mui/material';
import AuthService from '../Service/AuthService';
import { useNavigate } from 'react-router-dom';
import LocalizedStrings from 'react-localization';
import strings from '../localization.json';


const authService = new AuthService();

const useStyles = makeStyles((theme) => ({
  container: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    height: '100vh',
  },
  form: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    maxWidth: '300px',
  },
  textField: {
    marginBottom: theme.spacing(2),
  },
}));

const LoginPage = () => {
  const classes = useStyles();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const [language] = useState(localStorage.getItem('language') || 'en'); // Default language is English
  const [localization] = useState(()=>{
        return new LocalizedStrings(strings);

  });
  localization.setLanguage(language);

  const handleUsernameChange = (event) => {
    setUsername(event.target.value);
  };

  const handlePasswordChange = (event) => {
    setPassword(event.target.value);
  };
  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      await authService.login(username, password);
      navigate('/');
    } catch (error) {
      console.log('Error during login:', error);
      // Handle login error, display error message, etc.
    }

  };

  return (
    <Container className={classes.container}>
      <form className={classes.form} onSubmit={handleSubmit}>
        <Typography variant="h4" gutterBottom>
          {localization.loginTitle}
        </Typography>
        <TextField
          label="Username"
          variant="outlined"
          className={classes.textField}
          value={username}
          onChange={handleUsernameChange}
        />
        <TextField
          label="Password"
          variant="outlined"
          className={classes.textField}
          type="password"
          value={password}
          onChange={handlePasswordChange}
        />
        <Button type="submit" variant="contained" color="primary">
          {localization.signIn}
        </Button>
      </form>
    </Container>
  );
};

export default LoginPage;