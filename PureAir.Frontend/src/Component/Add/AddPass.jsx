import { useState, useEffect } from 'react';
import { FormGroup, FormControl, InputLabel, Input, Button, styled, Typography, Snackbar, Alert, Select, MenuItem} from '@mui/material';
import { addPass } from '../../Service/PassApi';
import { getUsers } from '../../Service/UserApi';
import { useNavigate } from 'react-router-dom';
import LocalizedStrings from 'react-localization';
import strings from '../../localization.json'

const initialValue = {
    id: '',
    employeeId: '',
}

const Container = styled(FormGroup)`
    width: 50%;
    margin: 5% 0 0 25%;
    & > div {
        margin-top: 20px;
`;

const AddPass = () => {
    const [pass, setUser] = useState(initialValue);
    const [employees, setEmployees] = useState([]);
    const {id, employeeId} = pass;
    const [error, setError] = useState(false);
    const [success, setSuccess] = useState(false);
    const [language] = useState(localStorage.getItem('language') || 'en'); // Default language is English
    const [localization] = useState(()=>{
          return new LocalizedStrings(strings);
    });
    localization.setLanguage(language);
    let navigate = useNavigate();

    const onValueChange = (e) => {
        setUser({...pass, [e.target.name]: e.target.value})
    }
    useEffect(() => {
        getEmployees();
    }, []);


    const getEmployees = async () => {
        let response = await getUsers();
        setEmployees(response.data);
    }

    const addPassDetails = async() => {
        let response = await addPass(pass);
        if(response!==undefined){
            if(response.status===200){
                setSuccess(true);
                navigate('/passes');
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
            <Typography variant="h4">{localization.addPassLabel}</Typography>
            <FormControl>
                <InputLabel htmlFor="my-input">{localization.passIdLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='id' value={id} id="my-input" aria-describedby="my-helper-text" />
            </FormControl>
            <FormControl>
            <InputLabel htmlFor="my-input">{localization.employeeIdLabel}</InputLabel>
            <Select
                name="employeeId"
                id="my-input"
                value={employeeId}
                label="Age"
                onChange={(e) => onValueChange(e)}
            >
            {employees.map((emp) => {
                return (
                    <MenuItem
                        key={`${emp.first_Name} ${emp.last_Name} ${emp.jobTitle}`}
                        value={emp.id}
                    >{`${emp.first_Name} ${emp.last_Name} ${emp.jobTitle}`}</MenuItem>
          );
        })}
        </Select>
                
            </FormControl>
                        <FormControl>
                <Button variant="contained" color="primary" onClick={() => addPassDetails()}>{localization.addPassLabel}</Button>
            </FormControl>
            <Snackbar open={success} autoHideDuration={6000} onClose={handleClose}>
                <Alert  severity="success" sx={{ width: '100%' }}>
                    Pass is added!
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

export default AddPass;