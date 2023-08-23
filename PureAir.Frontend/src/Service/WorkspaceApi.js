import axios from 'axios';

const workspaceUrl = 'http://localhost:7031/api/Workspaces';
axios.defaults.headers.common['Authorization'] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjBkOTBiNDE2LTIzN2MtNGJmNS1iNDJhLTQ5NzY5NTY2OWQ0NSIsInN1YiI6InJvc3R5c2xhdkBudXJlLnVhIiwiZW1haWwiOiJyb3N0eXNsYXZAbnVyZS51YSIsImp0aSI6ImZmNjk1OGQ4LTJhYjMtNDJjYi1hNjM2LWY4ZTcxMDYwNzY5ZCIsImlhdCI6MTY4NDE5NDAwOCwicm9sZSI6IkFkbWluIiwibmJmIjoxNjg0MTk0MDA4LCJleHAiOjE2ODY4NzI0MDh9.2weJ6KSQkYvozrvXxbvzM-CCebsouc6t01O3lJBBgIk";

export const getWorkspaces = async (id) => {
    id = id || '';
    try {
        return await axios.get(`${workspaceUrl}/${id}`);
    } catch (error) {
        console.log('Error while calling getWorkspaces api ', error);
    }
}

export const addWorkspace = async (workspace) => {
    return await axios.post(`${workspaceUrl}`, workspace).catch((error)=>{
        console.log({...error})
    });
}

export const deleteWorkspace = async (id) => {
    return await axios.delete(`${workspaceUrl}/${id}`).catch((error)=>{
        console.log({...error})
    });
}

export const editWorkspace = async (id, workspace) => {
    return await axios.put(`${workspaceUrl}/${id}`, workspace).catch((error)=>{
        console.log({...error})
    });
}