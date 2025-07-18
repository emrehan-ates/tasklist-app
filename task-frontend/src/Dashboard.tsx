import React from "react";
import ApiService from './helpers/ApiService.ts';
import './styles/dashboard.css';
import './styles/listcards.css'
import { useNavigate } from "react-router-dom";
import { useState, useEffect } from 'react';
import logo from "./assets/icon.png";
import type { List } from './helpers/ApiService.ts';
import ListPage from "./ListPage.tsx";
import useModal from "./helpers/useModal.tsx";


function Dashboard() {


    const navigate = useNavigate();
    const [lists, setLists] = useState<List[]>([]);
    const [asc, setAsc] = useState(true);
    const [deleteCount, setDelete] = useState(0);
    const addModal = useModal();
    const updateToggle = useModal();
    const [sameName, setSameName] = useState(false);

    interface ListDTO {
        user_id?: number;
        list_name?: string;
        list_description?: string;
    }

    const [newList, setNewList] = useState<ListDTO>({user_id : 0, list_name : "", list_description: ""});

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setNewList((prev) => ({ ...prev, [name]: value }));
    };

    useEffect(() => {

        ApiService.getAllList(asc)
            .then(res => setLists(res))
            .catch(err => console.error(err));
    }, [asc, deleteCount]);

    const handleDelete = (list_id: number): void => {
        ApiService.deleteList(list_id)
            .then(() => setDelete(prevCount => prevCount + 1))
    }

    const handleAdd = async (newlist: ListDTO): Promise<boolean> => {
        try {
            const res = await ApiService.addList(newlist.user_id, newlist.list_name, newlist.list_description);
            if(res){
                setNewList({ user_id: 0, list_name: "", list_description: "" });
                setDelete(prevCount => prevCount + 1);
            }
            setSameName(false);
            return res;
        } catch (err) {
            setSameName(true);
            console.error(err);
            return false;
        }
    }

    // const handleUpdate = async (uplist:ListDTO) : Promise<boolean> => {
    //     try {
    //         const res = 
    //     } catch (error) {
            
    //     }
    // }


    return (
        <>
            <div className="top-menu">
                <img src={logo} alt="logo" className="logo" ></img>
                <span  onClick={() => navigate('/home')} >Home</span>
                <span className="profile-button" onClick={() => navigate('/home')}>Profile</span>
            </div>

            <div className="list-cards-container">
                {
                    lists.map(list => (
                        <div className="list-card" key={list.list_id} onClick={() => navigate('/tasks', {state: {list}})}>
                            <button className="delete-btn" onClick={(e) => {
                                e.stopPropagation();
                                handleDelete(list.list_id)}}>X</button>
                            <button className="update-btn" onClick={(e) => {
                                e.stopPropagation();
                                updateToggle.toggle();
                            }}>O</button>
                            <h3 className="list-name">{list.list_name}</h3>
                            <p className="list-desc">{list.list_description}</p>
                        </div>
                    ))
                }
                <button className="add-btn" onClick={addModal.toggle}>
                    + Add List
                </button>
                <ListPage isOpen={addModal.isOpen} toggle={addModal.toggle}>

                    <h2>Add List</h2>
                    <label > List Name: </label>
                    <input
                        type="text"
                        name="list_name"
                        id="list_name"
                        placeholder="List name"
                        value={newList.list_name}
                        onChange={handleChange}
                    />
                    <label > List Description: </label>
                    <textarea
                        placeholder="Description"
                        name="list_description"
                        id="list_description"
                        value={newList.list_description}
                        onChange={handleChange}
                    />
                    <button className="verify-add" onClick={async () => {const res = await handleAdd(newList);
                        if(res){
                            addModal.toggle();
                        } else {
                            setSameName(true);
                        }
                        }}>Okay</button>
                    {sameName && 
                    <p className="samename-error">Cannot add list with same name!</p>
                    }
                </ListPage>

                <ListPage isOpen={updateToggle.isOpen} toggle={updateToggle.toggle}>
                <h2>Add List</h2>
                    <label > List Name: </label>
                    <input
                        type="text"
                        name="list_name"
                        id="list_name"
                        placeholder="List name"
                        value={newList.list_name}
                        onChange={handleChange}
                    />
                    <label > List Description: </label>
                    <textarea
                        placeholder="Description"
                        name="list_description"
                        id="list_description"
                        value={newList.list_description}
                        onChange={handleChange}
                    />
                    <button className="verify-add" onClick={async () => {const res = await handleAdd(newList);
                        if(res){
                            addModal.toggle();
                        } else {
                            setSameName(true);
                        }
                        }}>Okay</button>
                    {sameName && 
                    <p className="samename-error">Cannot add list with same name!</p>
                    }
                </ListPage>
            </div>

        </>
    );

}

export default Dashboard;