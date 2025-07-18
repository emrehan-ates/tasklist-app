import React from 'react';
import type { ReactNode } from 'react';
import './styles/listpage.css'

interface ModalType {
    children?: ReactNode;
    isOpen: boolean;
    toggle: () => void;
}

export default function Modal(props: ModalType){
    return (
        <>
            {props.isOpen && (

                <div className='modal-overlay' onClick={props.toggle}>

                    <div onClick={(e) => e.stopPropagation()} className='modal-box'>
                        {props.children}
                    </div>
                </div>
            )}
        
        </>
    );
}