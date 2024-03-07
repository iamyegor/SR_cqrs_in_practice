import { useState } from "react";
import Modal from "./Modal.tsx";

export default function App() {
    const [showModal, setShowModal] = useState(false);

    const openModal = () => {
        setShowModal(true);
    };

    const closeModal = () => {
        setShowModal(false);
    };

    return (
        <div className="container mx-auto p-6">
            <div className="flex justify-between mb-4">
                <div className="flex space-x-2">
                    <div className="space-x-1 flex items-center">
                        <p>Enrolled in:</p>
                        <select className="mr-2 p-1 border border-gray-300 rounded min-w-16">
                            <option></option>
                        </select>
                    </div>
                    <div className="space-x-1 flex items-center">
                        <p>Number of courses:</p>
                        <select className="mr-2 p-1 border border-gray-300 rounded min-w-16">
                            <option></option>
                        </select>
                    </div>
                    <div className="flex items-center">
                        <button
                            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-1 
                        px-4 rounded border border-blue-800"
                        >
                            Search
                        </button>
                    </div>
                </div>
                <div className="whitespace-nowrap">
                    <button
                        className="bg-green-500 hover:bg-green-700 text-white font-bold py-2 px-4 rounded mr-2"
                        onClick={openModal}
                    >
                        Add
                    </button>
                    <button className="bg-sky-500 hover:bg-sky-700 text-white font-bold py-2 px-4 rounded mr-2">
                        Edit Info
                    </button>
                    <button className="bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded">
                        Remove
                    </button>
                </div>
            </div>

            <div className="overflow-x-auto">
                <table className="min-w-full bg-white">
                    <thead className="bg-gray-800 text-white">
                        <tr>
                            <th className="w-1/4 min-w-[160px] text-left py-3 px-4 uppercase font-semibold text-sm">
                                Name
                            </th>
                            <th className="w-1/4 min-w-[160px] text-left py-3 px-4 uppercase font-semibold text-sm">
                                Email
                            </th>
                            <th className="w-1/4 min-w-[160px] text-left py-3 px-4 uppercase font-semibold text-sm">
                                First Course
                            </th>
                            <th className="w-1/4 min-w-[160px] text-left py-3 px-4 uppercase font-semibold text-sm">
                                Second Course
                            </th>
                        </tr>
                    </thead>
                    <tbody className="text-gray-700">
                        <tr>
                            <td className="text-left py-3 px-4">Alice</td>
                            <td className="text-left py-3 px-4">alice@gmail.com</td>
                            <td className="text-left py-3 px-4">
                                Calculus
                                <br />
                                Grade: A<br />
                                Credits: 3<br />
                                <button className="mt-2 text-white bg-blue-500 hover:bg-blue-700 font-medium py-1 px-3 rounded text-xs">
                                    Transfer
                                </button>
                                <button className="mt-2 ml-2 text-white bg-red-500 hover:bg-red-700 font-medium py-1 px-3 rounded text-xs">
                                    Disenroll
                                </button>
                            </td>
                            <td className="text-left py-3 px-4">
                                Composition
                                <br />
                                Grade: B<br />
                                Credits: 3<br />
                                <button className="mt-2 text-white bg-blue-500 hover:bg-blue-700 font-medium py-1 px-3 rounded text-xs">
                                    Transfer
                                </button>
                                <button className="mt-2 ml-2 text-white bg-red-500 hover:bg-red-700 font-medium py-1 px-3 rounded text-xs">
                                    Disenroll
                                </button>
                            </td>
                        </tr>
                        {/* More rows can be added here */}
                    </tbody>
                </table>
            </div>
            {showModal && <Modal onClose={closeModal} />}
        </div>
    );
}
