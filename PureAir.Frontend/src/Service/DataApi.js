import axios from 'axios';
import configData from "../config.json";

const workspaceUrl = configData.baseUrl+'WorkspaceDatas';
const token = localStorage.getItem('token');

if (token) {
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
}

export const getDatas = async (id) => {
    id = id || '';
    try {
        return await axios.get(`${workspaceUrl}/info/${id}`);
    } catch (error) {
        console.log('Error while calling getDatas api ', error);
    }
}

export const getDatasWorkspace = async (id, date) => {
    id = id || '';
    try {
        return await axios.get(`${workspaceUrl}/workspace/${id}/${date}`);
    } catch (error) {
        console.log('Error while calling getDatasWorkspace api ', error);
    }
}

export const addData = async (data) => {
    return await axios.post(`${workspaceUrl}`, data).catch((error)=>{
        console.log({...error})
    });
}

export const deleteData = async (id) => {
    return await axios.delete(`${workspaceUrl}/${id}`).catch((error)=>{
        console.log({...error})
    });
}

export const getDecision = async (id) => {
    return await axios.get(`${workspaceUrl}/decision/${id}`).catch((error)=>{
        console.log({...error})
    });
}

export const editData = async (id, data) => {
    return await axios.put(`${workspaceUrl}/${id}`, data).catch((error)=>{
        console.log({...error})
    });
}
