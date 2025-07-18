import React from "react";
import ApiService from './helpers/ApiService.ts';
import { useNavigate, useLocation } from "react-router-dom";
import { useState, useEffect } from 'react';
import ListPage from "./ListPage.tsx";
import useModal from "./helpers/useModal.tsx";
import './styles/taskspage.css'
import logo from './assets/icon.png'
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { deAT } from "date-fns/locale";
export default function TasksPage() {


    interface Task {
        task_id: number;
        list_id?: number;
        task_name?: string;
        task_description?: string;
        task_created?: string; 
        task_done?: boolean;
        deadline?: string; 
    }

    interface TaskDTO{
        list_id?: number;
        task_name?: string;
        task_description?: string;
        task_done?: boolean;
        deadline?: string; 
    }

    
    const [tasks, setTasks] = useState<Task[]>([]);
    const navigate = useNavigate();
    const location = useLocation();
    const [trigger, setTrigger] = useState(0);
    const taskModal = useModal();
    const updateModal = useModal();
    const [sameName, setSameName] = useState(false);
    


    const {list} = location.state || {};
    const id = list.list_id;
    const [upTask, setUpTask] = useState<Task>({
        task_id: 0,
        list_id: id,
        task_name: "",
        task_description: "",
        task_created: "",
        task_done: false,
        deadline: ""
    })
    
    const [newTask, setNewTask] = useState<TaskDTO>(
        {
            list_id : id,
            task_name: "",
            task_description: "",
            task_done: false,
            deadline: ""
        }
    )
    
    useEffect(() => {

        ApiService.getAllTasks(list.list_id, true)
            .then(res => setTasks(res))
            .catch(err => console.error(err));
    }, [trigger]); //buraya bikaç condition ekle q
    
    const handleDelete = async (id:number):Promise<Boolean> => {

        try {
            ApiService.deleteTask(id)
            .then(() => setTrigger((prev)=> ++prev));
            return true;
        } catch (error) {
            console.error(error);
            return false;
        }
        
    } 

    const handleAdd = async (list_id:number, task_name:string, task_description:string, task_done:boolean, deadline:string): Promise<Boolean> => {

        try {
            const fo = await ApiService.addTask(list_id, task_name, task_description, task_done, deadline).then(() => setTrigger((prev) => prev++)).then(() => setNewTask({
                list_id : id,
                task_name: "",
                task_description: "",
                task_done: false,
                deadline: ""
            }))
            return true;
                        
        } catch (err) {
            console.error(err);
            return false;
        }
    }

    const handleDone = async (id:number): Promise<Boolean> =>{
        try {
            const fo = await ApiService.setTaskDone(id).then(() => setTrigger((prev) => ++prev));
            return true;
        } catch (error) {
            console.error(error);
            return false;
        }
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
            const { name, value } = e.target;
            setNewTask((prev) => ({ ...prev, [name]: value }));
    }

    const handleUpChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const {name, value} = e.target;
        setUpTask((prev) => ({...prev, [name]: value}));
    }

    const handleUpdate = async (up:Task): Promise<boolean> => {
        try {
            const fo = await ApiService.updateTask(up).then(() => setUpTask({
                task_id: 0,
                list_id: id,
                task_name: "",
                task_description: "",
                task_created: "",
                task_done: false,
                deadline: ""
            }));
            return true;
        } catch (error) {
            console.error(error);
            return false;
        }
    }

    return(
        <>
        <div className="top-menu">
            <img src={logo} alt="logo" className="logo" onClick={() => navigate('/home')} ></img>
            <span  onClick={() => navigate('/home')} >Home</span>
            <span className="profile-button" onClick={() => navigate('/home')}>Profile</span>
        </div>


        
        <div className="tasks-container">
            <ul className="task-list">
                {
                    tasks.map(task => (
                        <li className="tasks-item" key={task.task_id}>
                            <button
                                className="delete-btn"
                                onClick={e => {
                                    e.stopPropagation();
                                    handleDelete(task.task_id);
                                }}
                            >
                                ×
                            </button>
                            <button
                                className="update-btn"
                                onClick={e => {
                                    e.stopPropagation();
                                    setUpTask({
                                        task_id: task.task_id,
                                        list_id: id,
                                        task_description: task.task_description,
                                        task_created: task.task_created,
                                        task_name: task.task_name,
                                        task_done: task.task_done,
                                        deadline: task.deadline
                                    })
                                    updateModal.toggle();
                                }}
                                disabled={task.task_done}
                            >
                                ✎
                            </button>
                            <h4>{task.task_name}</h4>
                            <p>{task.task_description}</p>
                            <span>Created: {task.task_created}</span>
                            <span>Done: {task.task_done ? "Yes" : "No"}</span>
                            <span>Deadline: {task.deadline}</span>
                            <button
                                className="done-btn"
                                onClick={e => {
                                    e.stopPropagation();
                                    handleDone(task.task_id);
                                }}
                                disabled={task.task_done}
                            >
                                {task.task_done ? "Completed" : "Set Done"}
                            </button>
                        </li>
                    ))
                }
                <button className="add-btn" onClick={taskModal.toggle}>+ Add Task</button>

                <ListPage isOpen={taskModal.isOpen} toggle={taskModal.toggle}>

                <h2>Add List</h2>
                    <label > List Name: </label>
                    <input
                        type="text"
                        name="task_name"
                        id="task_name"
                        placeholder="Task name"
                        value={newTask.task_name}
                        onChange={handleChange}
                    />
                    <label > List Description: </label>
                    <textarea
                        placeholder="Description"
                        name="task_description"
                        id="task_description"
                        value={newTask.task_description}
                        onChange={handleChange}
                    />

                    <label>Deadline:</label>
                    <DatePicker
                        selected={newTask.deadline ? new Date(newTask.deadline) : null}
                        onChange={date => {
                            if (date) {
                                // Format as YYYY-MM-DDTHH:mm:00
                                const year = date.getFullYear();
                                const month = String(date.getMonth() + 1).padStart(2, '0');
                                const day = String(date.getDate()).padStart(2, '0');
                                const hours = String(date.getHours()).padStart(2, '0');
                                const minutes = String(date.getMinutes()).padStart(2, '0');
                                const formatted = `${year}-${month}-${day}T${hours}:${minutes}:00`;
                                setNewTask(prev => ({
                                    ...prev,
                                    deadline: formatted
                                }));
                            } else {
                                setNewTask(prev => ({
                                    ...prev,
                                    deadline: ""
                                }));
                            }
                        }}
                        showTimeSelect
                        timeFormat="HH:mm"
                        timeIntervals={5}
                        dateFormat="yyyy-MM-dd HH:mm"
                        placeholderText="Select deadline"
                    />
                    <button className="verify-add" onClick={async () => {const res = await handleAdd(id, newTask.task_name || "", newTask.task_description || "", false, newTask.deadline || "");
                        if(res){
                            taskModal.toggle();
                        } else {
                            setSameName(true);
                        }
                        setTrigger((prev) => prev = prev+1)
                        }}>Okay</button>
                    {sameName && 
                    <p className="samename-error">Cannot add list with same name!</p>
                    }
                </ListPage>

                <ListPage isOpen={updateModal.isOpen} toggle={updateModal.toggle}>

                <h2>Add List</h2>
                    <label > List Name: </label>
                    <input
                        type="text"
                        name="task_name"
                        id="task_name"
                        placeholder="Task name"
                        value={upTask.task_name}
                        onChange={handleUpChange}
                    />
                    <label > List Description: </label>
                    <textarea
                        placeholder="Description"
                        name="task_description"
                        id="task_description"
                        value={upTask.task_description}
                        onChange={handleUpChange}
                    />

                    <label>Deadline:</label>
                    <DatePicker
                        selected={upTask.deadline ? new Date(upTask.deadline) : null}
                        onChange={date => {
                            if (date) {
                                // Format as YYYY-MM-DDTHH:mm:00
                                const year = date.getFullYear();
                                const month = String(date.getMonth() + 1).padStart(2, '0');
                                const day = String(date.getDate()).padStart(2, '0');
                                const hours = String(date.getHours()).padStart(2, '0');
                                const minutes = String(date.getMinutes()).padStart(2, '0');
                                const formatted = `${year}-${month}-${day}T${hours}:${minutes}:00`;
                                setUpTask(prev => ({
                                    ...prev,
                                    deadline: formatted
                                }));
                            } else {
                                setUpTask(prev => ({
                                    ...prev,
                                    deadline: ""
                                }));
                            }
                        }}
                        showTimeSelect
                        timeFormat="HH:mm"
                        timeIntervals={5}
                        dateFormat="yyyy-MM-dd HH:mm"
                        placeholderText="Select deadline"
                    />
                    <button className="verify-add" onClick={async () => {const res = await handleUpdate(upTask);
                        if(res){
                            updateModal.toggle();
                        } else {
                            setSameName(true);
                        }
                        setTrigger((prev) => prev = prev+1)
                        }}>Okay</button>
                    {sameName && 
                    <p className="samename-error">Cannot add list with same name!</p>
                    }
                </ListPage>
            </ul>
        </div>
        

        
        </>
    );
}