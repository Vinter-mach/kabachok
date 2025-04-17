import {useState} from 'react';
import '../styles/Group.css';
import {useNavigate} from 'react-router-dom';

function Group() {
    const [inputValue, setInputValue] = useState('');
    const navigate = useNavigate();

    const handleClick = () => {
        navigate('/test');
    };

    const isButtonDisabled = inputValue.trim() === '';

    return (
        <div className="background-wrapper"> {/* Новая обёртка */}
            <div className="container">
                <h2 className="title">добавить группу</h2>
                <input
                    type="text"
                    value={inputValue}
                    onChange={(e) => setInputValue(e.target.value)}
                    placeholder="+"
                    className="input-field"
                />
                <button
                    onClick={handleClick}
                    className="button"
                    disabled={isButtonDisabled}
                >
                    добавить
                </button>
            </div>
        </div>
    );
}


export default Group;