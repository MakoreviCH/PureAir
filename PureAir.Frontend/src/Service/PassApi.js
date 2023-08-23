import axios from 'axios';

const passUrl = 'http://localhost:7031/api/EmployeePasses';
axios.defaults.headers.common['Authorization'] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjBkOTBiNDE2LTIzN2MtNGJmNS1iNDJhLTQ5NzY5NTY2OWQ0NSIsInN1YiI6InJvc3R5c2xhdkBudXJlLnVhIiwiZW1haWwiOiJyb3N0eXNsYXZAbnVyZS51YSIsImp0aSI6ImZmNjk1OGQ4LTJhYjMtNDJjYi1hNjM2LWY4ZTcxMDYwNzY5ZCIsImlhdCI6MTY4NDE5NDAwOCwicm9sZSI6IkFkbWluIiwibmJmIjoxNjg0MTk0MDA4LCJleHAiOjE2ODY4NzI0MDh9.2weJ6KSQkYvozrvXxbvzM-CCebsouc6t01O3lJBBgIk";

export const getPasses = async (id) => {
    id = id || '';
    try {
        return await axios.get(`${passUrl}/${id}`);
    } catch (error) {
        console.log('Error while calling getUsers api ', error);
    }
}

export const addPass = async (pass) => {
    return await axios.post(`${passUrl}`, pass).catch((error)=>{
        console.log({...error})
    });
}

export const deletePass = async (id) => {
    return await axios.delete(`${passUrl}/${id}`).catch((error)=>{
        console.log({...error})
    });
}

export const editPass = async (id, pass) => {
    return await axios.put(`${passUrl}/${id}`, pass)
}