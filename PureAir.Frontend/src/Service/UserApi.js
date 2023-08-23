import axios from 'axios';

const usersUrl = 'http://localhost:7031/api/Employees';
axios.defaults.headers.common['Authorization'] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjBkOTBiNDE2LTIzN2MtNGJmNS1iNDJhLTQ5NzY5NTY2OWQ0NSIsInN1YiI6InJvc3R5c2xhdkBudXJlLnVhIiwiZW1haWwiOiJyb3N0eXNsYXZAbnVyZS51YSIsImp0aSI6ImZmNjk1OGQ4LTJhYjMtNDJjYi1hNjM2LWY4ZTcxMDYwNzY5ZCIsImlhdCI6MTY4NDE5NDAwOCwicm9sZSI6IkFkbWluIiwibmJmIjoxNjg0MTk0MDA4LCJleHAiOjE2ODY4NzI0MDh9.2weJ6KSQkYvozrvXxbvzM-CCebsouc6t01O3lJBBgIk";

export const getUsers = async (id) => {
    id = id || '';
    try {
        return await axios.get(`${usersUrl}/${id}`);
    } catch (error) {
        console.log('Error while calling getUsers api ', error);
    }
}

export const addUser = async (user) => {
    return await axios.post(`http://localhost:7031/api/Authentication/Register`, user).catch((error)=>{
        console.log({...error})
    });
}

export const deleteUser = async (id) => {
    return await axios.delete(`${usersUrl}/${id}`);
}

export const editUser = async (id, user) => {
    return await axios.put(`${usersUrl}/${id}`, user).catch((error)=>{
        console.log({...error})
    });
}