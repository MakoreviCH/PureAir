import { useState } from 'react';
import { FormGroup, FormControl, InputLabel, Input, Button, styled, Typography, Snackbar, Alert } from '@mui/material';
import { addUser } from '../../Service/UserApi';
import { useNavigate } from 'react-router-dom';
import LocalizedStrings from 'react-localization';
import strings from '../../localization.json'
const initialValue = {
    first_Name: '',
    last_Name: '',
    phone_Number: '',
    jobTitle: '',
    email: '',
    password: ''
}

const Container = styled(FormGroup)`
    width: 50%;
    margin: 5% 0 0 25%;
    & > div {
        margin-top: 20px;
`;

const AddUser = () => {
    const [user, setUser] = useState(initialValue);
    const {email, password, first_Name, last_Name, phoneNumber, jobTitle } = user;
    const [error, setError] = useState(false);
    const [success, setSuccess] = useState(false);
    const [language] = useState(localStorage.getItem('language') || 'en'); // Default language is English
    const [localization] = useState(()=>{
          return new LocalizedStrings(strings);
    });
    localization.setLanguage(language);
    let navigate = useNavigate();

    const onValueChange = (e) => {
        setUser({...user, [e.target.name]: e.target.value})
    }


    const addUserDetails = async() => {
        let response = await addUser(user);
        if(response!==undefined){
            if(response.status===200){
                setSuccess(true);
                navigate('/users');
            }
            else
            setError(true);            
        }
        else
            setError(true);
    }
    const handleClose = (event, reason) => {
        if (reason === 'clickaway') {
          return;
        }
        setSuccess(false);
        setError(false);
      };
    return (
        
        <Container>
            <Typography variant="h4">{localization.addUserLabel}</Typography>
            <FormControl>
                <InputLabel htmlFor="my-input">Email</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='email' value={email} id="my-input" aria-describedby="my-helper-text" />
            </FormControl>
            <FormControl>
                <InputLabel htmlFor="my-input">{localization.passwordLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='password' value={password} id="my-input" aria-describedby="my-helper-text" />
            </FormControl>
            <FormControl>
                <InputLabel htmlFor="my-input">{localization.firstNameLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='first_Name' value={first_Name} id="my-input" aria-describedby="my-helper-text" />
            </FormControl>
            <FormControl>
                <InputLabel htmlFor="my-input">{localization.lastNameLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='last_Name' value={last_Name} id="my-input" aria-describedby="my-helper-text" />
            </FormControl>
            <FormControl>
                <InputLabel htmlFor="my-input">{localization.phoneLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='phoneNumber' value={phoneNumber} id="my-input" aria-describedby="my-helper-text" />
            </FormControl>
            <FormControl>
                <InputLabel htmlFor="my-input">{localization.jobLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='jobTitle' value={jobTitle} id="my-input" aria-describedby="my-helper-text" />
            </FormControl>
            <FormControl>
                <Button variant="contained" color="primary" onClick={() => addUserDetails()}>{localization.addUserLabel}</Button>
            </FormControl>
            <Snackbar open={success} autoHideDuration={6000} onClose={handleClose}>
                <Alert  severity="success" sx={{ width: '100%' }}>
                    Employee is added!
                </Alert>
            </Snackbar>
            <Snackbar open={error} autoHideDuration={6000} onClose={handleClose}>
                <Alert  severity="error" sx={{ width: '100%' }}>
                    Error! There are some problems with adding.
                </Alert>
            </Snackbar>
        </Container>
    )
}

export default AddUser;