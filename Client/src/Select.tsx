import React from "react";

interface SelectProps {
    label: string;
    name: string;
    value: string;
    options: string[];
    onChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
}

const Select: React.FC<SelectProps> = ({ label, name, value, options, onChange }) => {
    return (
        <div>
            <label className="block mb-2">{label}</label>
            <select
                name={name}
                className="border-2 border-gray-300 p-2 w-full rounded-md focus:border-blue-500 focus:outline-none"
                value={value}
                onChange={onChange}
            >
                {options.map((option, index) => (
                    <option key={index} value={option}>
                        {option}
                    </option>
                ))}
            </select>
        </div>
    );
};

export default Select;
