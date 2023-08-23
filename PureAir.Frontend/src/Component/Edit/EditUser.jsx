import { useState, useEffect } from 'react';
import LocalizedStrings from 'react-localization';
import strings from '../../localization.json'
import { FormGroup, FormControl, InputLabel, Input, Button, styled, Typography, Snackbar, Alert} from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import { getUsers, editUser } from '../../Service/UserApi';

const initialValue = {
    first_Name: '',
    last_Name: '',
    phone_Number: '',
    jobTitle: '',
    password: ''
}

const Container = styled(FormGroup)`
    width: 50%;
    margin: 5% 0 0 25%;
    & > div {
        margin-top: 20px
`;


const EditUser = () => {
    const [user, setUser] = useState(initialValue);
    const { first_Name, last_Name, phoneNumber, jobTitle, password } = user;
    const { id } = useParams();
    const [error, setError] = useState(false);
    const [success, setSuccess] = useState(false);
    let navigate = useNavigate();

    const [language] = useState(localStorage.getItem('language') || 'en'); // Default language is English
    const [localization] = useState(() => {
        return new LocalizedStrings(strings);
    });
    localization.setLanguage(language);

    useEffect(() => {
        loadUserDetails();
    }, []);

    const loadUserDetails = async() => {
        const response = await getUsers(id);
        setUser(response.data);
    }


    const editUserDetails = async() => {
        const response = await editUser(id, user);
        if(response.status===204){
            setSuccess(true);
            navigate('/users');
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
    const onValueChange = (e) => {
        console.log(e.target.value);
        setUser({...user, [e.target.name]: e.target.value})
    }

    return (
        <Container>
            <Typography variant="h4">{localization.editInfoLabel} {id}</Typography>
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
                <InputLabel htmlFor="my-input">{localization.passwordLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='password' value={password} id="my-input" aria-describedby="my-helper-text" />
            </FormControl>
            <FormControl>
                <Button variant="contained" color="primary" onClick={() => editUserDetails()}>{localization.confirmEditLabel}</Button>
            </FormControl>
            <Snackbar open={success} autoHideDuration={6000}  onClose={handleClose}>
                <Alert  severity="success" sx={{ width: '100%' }}>
                    Info is updated!
                </Alert>
            </Snackbar>
            <Snackbar open={error} autoHideDuration={6000} onClose={handleClose}>
                <Alert  severity="error" sx={{ width: '100%' }}>
                    Error! There are some problems with this data.
                </Alert>
            </Snackbar>
        </Container>
    )
}

export default EditUser;