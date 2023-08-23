import { useState } from 'react';
import { FormGroup, FormControl, InputLabel, Input, Button, styled, Typography , Snackbar, Alert } from '@mui/material';
import { addWorkspace } from '../../Service/WorkspaceApi';
import { useNavigate } from 'react-router-dom';
import LocalizedStrings from 'react-localization';
import strings from '../../localization.json'
const initialValue = {
    workspaceName: '',
    temperatureThreshold: '',
    humidityThreshold: '',
    gasThreshold: ''
}

const Container = styled(FormGroup)`
    width: 50%;
    margin: 5% 0 0 25%;
    & > div {
        margin-top: 20px;
`;

const AddWorkspace = () => {
    const [workspace, setWorkspace] = useState(initialValue);
    const { workspaceName, temperatureThreshold, humidityThreshold, gasThreshold } = workspace;
    const [error, setError] = useState(false);
    const [success, setSuccess] = useState(false);
    const [language] = useState(localStorage.getItem('language') || 'en'); // Default language is English
    const [localization] = useState(()=>{
          return new LocalizedStrings(strings);
    });
    localization.setLanguage(language);
    let navigate = useNavigate();

    const onValueChange = (e) => {
        setWorkspace({...workspace, [e.target.name]: e.target.value})
    }

    const addWorkspaceDetails = async() => {
        let response = await addWorkspace(workspace);
        if(response!==undefined){
            if(response.status===200){
                setSuccess(true);
                navigate('/workspaces');
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
            <Typography variant="h4">{localization.addWorkspaceLabel}</Typography>
            <FormControl>
                <InputLabel htmlFor="my-input">{localization.workspaceNameLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='workspaceName' value={workspaceName} id="my-input" />
            </FormControl>
            <FormControl>
                <InputLabel htmlFor="my-input">{localization.temperatureThresholdLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='temperatureThreshold' value={temperatureThreshold} id="my-input" />
            </FormControl>
            <FormControl>
                <InputLabel htmlFor="my-input">{localization.humidityThresholdLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='humidityThreshold' value={humidityThreshold} id="my-input"/>
            </FormControl>
            <FormControl>
                <InputLabel htmlFor="my-input">{localization.gasThresholdLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='gasThreshold' value={gasThreshold} id="my-input" />
            </FormControl>
            <FormControl>
                <Button variant="contained" color="primary" onClick={() => addWorkspaceDetails()}>{localization.addWorkspaceLabel}</Button>
            </FormControl>
            <Snackbar open={success} autoHideDuration={6000} onClose={handleClose}>
                <Alert  severity="success" sx={{ width: '100%' }}>
                    Workspace is added!
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

export default AddWorkspace;