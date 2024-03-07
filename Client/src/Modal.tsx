import React, { useState } from "react";
import Input from "./Input";
import TextArea from "./TextArea";
import Select from "./Select";

interface ModalProps {
    onClose: () => void;
    onSubmit: (formData: FormData) => void;
}

interface FormData {
    name: string;
    email: string;
    firstCourse: string;
    firstCourseGrade: string;
    firstCourseDisenrollmentComment: string;
    secondCourse: string;
    secondCourseGrade: string;
    secondCourseDisenrollmentComment: string;
}

const Modal: React.FC<ModalProps> = ({ onClose }) => {
    const [formData, setFormData] = useState<FormData>({
        name: "",
        email: "",
        firstCourse: "",
        firstCourseGrade: "",
        firstCourseDisenrollmentComment: "",
        secondCourse: "",
        secondCourseGrade: "",
        secondCourseDisenrollmentComment: "",
    });

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>,
    ) => {
        const { name, value } = e.target;
        setFormData((prevFormData) => ({ ...prevFormData, [name]: value }));
    };

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        console.log(formData);
        onClose();
    };

    const courses = [
        "",
        "Calculus",
        "Chemistry",
        "Composition",
        "Literature",
        "Trigonometry",
        "Microeconomics",
        "Macroeconomics",
    ];
    const grades = ["", "A", "B", "C", "D", "F"];

    return (
        <>
            <div className="fixed inset-0 bg-black bg-opacity-30 backdrop-blur z-40" />
            <div className="fixed inset-0 flex items-center justify-center z-50">
                <div className="bg-white rounded-lg shadow-2xl p-6 w-3/4 border">
                    <form onSubmit={handleSubmit}>
                        <div className="mb-4">
                            <Input
                                label="Name"
                                name="name"
                                value={formData.name}
                                onChange={handleChange}
                                placeholder="Bob"
                            />
                        </div>
                        <div className="mb-4">
                            <Input
                                label="Email"
                                name="email"
                                value={formData.email}
                                onChange={handleChange}
                                type="email"
                                placeholder="bob@outlook.com"
                            />
                        </div>
                        <div className="mb-4">
                            <Select
                                label="First course"
                                name="firstCourse"
                                value={formData.firstCourse}
                                options={courses}
                                onChange={handleChange}
                            />
                        </div>
                        {formData.firstCourse ? (
                            <div className="mb-4">
                                <Select
                                    label="First course grade"
                                    name="firstCourseGrade"
                                    value={formData.firstCourseGrade}
                                    options={grades}
                                    onChange={handleChange}
                                />
                            </div>
                        ) : (
                            <div className="mb-4">
                                <TextArea
                                    label="Disenrollment comment"
                                    name="firstCourseDisenrollmentComment"
                                    value={formData.firstCourseDisenrollmentComment}
                                    onChange={handleChange}
                                />
                            </div>
                        )}
                        <div className="mb-4">
                            <Select
                                label="Second course"
                                name="secondCourse"
                                value={formData.secondCourse}
                                options={courses}
                                onChange={handleChange}
                            />
                        </div>
                        {formData.secondCourse ? (
                            <div className="mb-4">
                                <Select
                                    label="Second course grade"
                                    name="secondCourseGrade"
                                    value={formData.secondCourseGrade}
                                    options={grades}
                                    onChange={handleChange}
                                />
                            </div>
                        ) : (
                            <div className="mb-4">
                                <TextArea
                                    label="Disenrollment comment"
                                    name="secondCourseDisenrollmentComment"
                                    value={formData.secondCourseDisenrollmentComment}
                                    onChange={handleChange}
                                />
                            </div>
                        )}
                        <button
                            type="submit"
                            className="py-2 px-4 bg-blue-500 text-white font-bold rounded-lg shadow-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-400 focus:ring-opacity-50"
                        >
                            OK
                        </button>
                        <button
                            type="button"
                            className="ml-2 py-2 px-4 bg-gray-500 text-white font-bold rounded-lg shadow-md hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-gray-400 focus:ring-opacity-50"
                            onClick={onClose}
                        >
                            Cancel
                        </button>
                    </form>
                </div>
            </div>
        </>
    );
};

export default Modal;
