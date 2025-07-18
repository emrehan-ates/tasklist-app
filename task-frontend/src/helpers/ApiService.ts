
import axios from 'axios';
import type { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios';
//interface for the Helper
const axiosInstance: AxiosInstance = axios.create({
    baseURL: 'http://localhost:5026/api/', // 🔁 Replace with your backend URL
    timeout: 5000,
    headers: {
      'Content-Type': 'application/json',
    },
  });
  axiosInstance.interceptors.request.use(config => {
    const token = localStorage.getItem('token');
    if(token && config.headers){
        config.headers.Authorization= `Bearer ${token}`;

    }

    return config;
  });

  export interface User {
    user_id?: number;
    user_name?: string;
    user_surname?: string;
    user_email?: string;
    user_birthdate?: string; // ISO 8601 format: 'yyyy-MM-dd'
    user_password: string;
    user_created?: string; // ISO string, from DateTime?
  }

  export interface List {
    list_id: number;
    user_id?: number;
    list_name?: string;
    list_description?: string;
    list_created?: string; // ISO format: '2025-06-27T12:34:56Z'
  }

  export interface Task {
    task_id: number;
    list_id?: number;
    task_name?: string;
    task_description?: string;
    task_created?: string; // ISO 8601 datetime string
    task_done?: boolean;
    deadline?: string; // also an ISO string like '2025-07-01T15:30:00'
  }

let ApiService = {
  

  //lets start with the user controller
  login: async (user_email:string, user_password:string ) =>{
    try{
        const res = await axiosInstance.post('user/login', {user_email, user_password});
        const token = res.data.token;
        if(token){
            localStorage.setItem('token',token);
            return true;
        }
        return false;
    } catch(err){
        console.error('Login failed: ', err);
        return false;
    }
  },

  register:  async (user_name:string, user_surname:string, user_email:string, user_birthdate:string, user_password:string): Promise<boolean> => {
    try{
        const res = await axiosInstance.post('user/register', {user_name, user_surname, user_email, user_birthdate, user_password});
        console.log(res.data);
        return true;
    } catch (err){
        console.error('kaydolamadı hahaha',err);
        return false;
    }
  },

  deleteUser: async () => {
    try{
        const res = await axiosInstance.delete('user/delete');
        console.log(res.data);
        return true;


    } catch(err){
        console.error('Couldnt delete', err);
        return false;
    }
  },

  getProfile: async (): Promise<User> => {
    try {
        const res = await axiosInstance.get('user/getprofile');
        console.log(res.data);
        return res.data;
    } catch(err){
        console.error('couldnt get it: ', err);
        throw Error;
    }
  },

  //List actions are here

  getAllList: async (asc:boolean): Promise<List[]> => {
    try {
        const res = await axiosInstance.get('list/getall', {params:{asc:asc}});
        return res.data;
    } catch (error) {
        console.error('couldnt get it: ', error);
        throw Error;
    }
  },

  getListByName: async (name:string): Promise<List> => {
    try {
        const res = await axiosInstance.get('list/get', {params:{name:name}});
        console.log(res.data);
        return res.data;
    } catch (error) {
        console.error('couldnt get it: ', error);
        throw Error;
    }
  },

  addList: async (user_id:number | undefined, list_name:string | undefined, list_description:string | undefined): Promise<boolean> => {
    try {
        const res = await axiosInstance.post('list/add', {user_id, list_name, list_description});
        console.log(res);
        return true;
    } catch (err) {
        console.error('Couldnt add the list: ', err);
        return false;
    }
  },

  deleteList: async (id:number): Promise<boolean> => {
    try {
        const res = await axiosInstance.delete(`list/delete/${id}`);
        console.log(res.data);
        return true;
    } catch (error) {
        console.error('couldnt delete list: ', error);
        return false;
    }
  },

  //Task Endpoint Now GOOO


  getAllTasks: async (id:number, asc:boolean): Promise<Task[]> => {
    try {
        const res = await axiosInstance.get('task/getall', {params:{id, asc}});
        return res.data;
    } catch (error) {
        console.error('Couldnt get all tasks: ', error);
        throw error;
    }
  },

  getTaskByName: async (id:number, name:string): Promise<Task[]> => {
    try {
        const res = await axiosInstance.get(`task/get/${id}`, {params: {name}});
        return res.data;
    } catch (error) {
        console.error('Couldnt get tasks: ', error);
        throw error;
    }
  },

  FilterByDeadline: async (id:number, low?:string, high?:string): Promise<Task[]> => {
    try {
        const res = await axiosInstance.get(`task/deadline/${id}`, {params: {low, high}});
        return res.data;
    } catch (error) {
        console.error('Couldnt get tasks: ', error);
        throw error;
    }
  },

  addTask: async (list_id:number | undefined, task_name:string | undefined, task_description:string | undefined, task_done:boolean, deadline:string | undefined): Promise<boolean> => {
    try {
        const res = await axiosInstance.post('task/addtask', {list_id, task_name, task_description, task_done, deadline});

        if(res.data == 200)
            return true;
        else 
            return false;
    } catch (error) {
        console.error('Couldnt add it: ', error);
        return false;
    }
  },

  updateTask: async (up:Task): Promise<boolean> => {
    try {
        const res = await axiosInstance.put('task/update', up);
        console.log(res.data);
        return true;
    } catch (error) {
        console.error('Couldnt update the task: ', error);
        return false;
    }
  },

  deleteTask: async (id:number): Promise<boolean> => {
    try {
        const res = await axiosInstance.delete(`task/delete/${id}`);
        console.log(res.data);
        return true;
    } catch (error) {
        console.error('Couldnt delete task: ', error);
        return false;
    }
  },

  setTaskDone: async (id:number): Promise<boolean> => {
    try {
      const res = await axiosInstance.put(`task/setdone/${id}`);
      console.log(res.data);
      return true;
    } catch (error) {
      console.error('Couldnt update task: ', error);
      return false;
    }
  }
}

export default ApiService;

