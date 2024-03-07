import React from "react";

interface InputProps {
    label: string;
    name: string;
    value: string;
    type?: string;
    placeholder?: string;
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

const Input: React.FC<InputProps> = ({
    label,
    name,
    value,
    type = "text",
    placeholder = "",
    onChange,
}) => {
    return (
        <div>
            <label className="block mb-2">{label}</label>
            <input
                type={type}
                name={name}
                className="border-2 border-gray-300 p-2 w-full rounded-md focus:border-blue-500 focus:outline-none"
                placeholder={placeholder}
                value={value}
                onChange={onChange}
            />
        </div>
    );
};

export default Input;
