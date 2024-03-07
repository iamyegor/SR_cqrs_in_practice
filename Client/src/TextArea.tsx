import React from "react";

interface TextAreaProps {
    label: string;
    name: string;
    value: string;
    rows?: number;
    onChange: (e: React.ChangeEvent<HTMLTextAreaElement>) => void;
}

const TextArea: React.FC<TextAreaProps> = ({ label, name, value, rows = 3, onChange }) => {
    return (
        <div>
            <label className="block mb-2">{label}</label>
            <textarea
                name={name}
                className="border-2 border-gray-300 p-2 w-full rounded-md focus:border-blue-500 focus:outline-none"
                rows={rows}
                value={value}
                onChange={onChange}
            />
        </div>
    );
};

export default TextArea;
