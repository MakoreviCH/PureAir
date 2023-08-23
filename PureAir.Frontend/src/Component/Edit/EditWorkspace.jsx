import { useState, useEffect } from 'react';
import LocalizedStrings from 'react-localization';
import strings from '../../localization.json'
import { FormGroup, FormControl, InputLabel, Input, Button, styled, Typography, Alert, Snackbar } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import { editWorkspace, getWorkspaces } from '../../Service/WorkspaceApi';

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
        margin-top: 20px
`;


const EditWorkspace = () => {
    const [workspace, setWorkspace] = useState(initialValue);
    const { workspaceName, temperatureThreshold, humidityThreshold, gasThreshold } = workspace;

    const [error, setError] = useState(false);
    const [success, setSuccess] = useState(false);

    const { id } = useParams();

    const [language] = useState(localStorage.getItem('language') || 'en'); // Default language is English
    const [localization] = useState(() => {
        return new LocalizedStrings(strings);
    });
    localization.setLanguage(language);

    let navigate = useNavigate();

    useEffect(() => {
        loadWorkspaceDetails();
    }, []);

    const loadWorkspaceDetails = async () => {
        const response = await getWorkspaces(id);
        setWorkspace(response.data);
    }

    const editWorkspaceDetails = async () => {
        let response = await editWorkspace(id, workspace);
        if (response !== undefined) {
            if (response.status === 204) {
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
    const onValueChange = (e) => {
        console.log(e.target.value);
        setWorkspace({ ...workspace, [e.target.name]: e.target.value })
    }

    return (
        <Container>
            <Typography variant="h4">{localization.editInfoLabel} {id}</Typography>
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
                <Input onChange={(e) => onValueChange(e)} name='humidityThreshold' value={humidityThreshold} id="my-input" />
            </FormControl>
            <FormControl>
                <InputLabel htmlFor="my-input">{localization.gasThresholdLabel}</InputLabel>
                <Input onChange={(e) => onValueChange(e)} name='gasThreshold' value={gasThreshold} id="my-input" />
            </FormControl>
            <FormControl>
                <Button variant="contained" color="primary" onClick={() => editWorkspaceDetails()}>{localization.confirmEditLabel}</Button>
            </FormControl>
            <Snackbar open={success} autoHideDuration={6000} onClose={handleClose}>
                <Alert severity="success" sx={{ width: '100%' }}>
                    Info is updated!
                </Alert>
            </Snackbar>
            <Snackbar open={error} autoHideDuration={6000} onClose={handleClose}>
                <Alert severity="error" sx={{ width: '100%' }}>
                    Error! There are some problems with this data.
                </Alert>
            </Snackbar>
        </Container>
    )
}

export default EditWorkspace;