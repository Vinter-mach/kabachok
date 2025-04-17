import {useState} from 'react';
import {useNavigate} from 'react-router-dom';

function SelectionPage() {
    const navigate = useNavigate();
    const [selectedOption, setSelectedOption] = useState('');
    //добавление новых опций
    const [newOption, setNewOption] = useState('');
    //старые опции
    const [options, setOptions] = useState([]);

    // Добавление нового варианта
    const handleAddOption = () => {
        if (newOption.trim() && !options.includes(newOption)) {
            setOptions([...options, newOption]);
            setNewOption('');
        }
    };

    // Переход на следующую страницу
    const handleNavigate = () => {
        if (selectedOption) {
            navigate('/group', {state: {selectedOption}});
        }
    };

    return (
        <div className="selection-container">
            <h2>Выберите или добавьте вариант</h2>

            <div className="form-group">
                <label htmlFor="options">Доступные варианты:</label>
                <select
                    id="options"
                    value={selectedOption}
                    onChange={(e) => setSelectedOption(e.target.value)}
                    className="form-control"
                >
                    <option value="">-- Выберите вариант --</option>
                    {options.map((option, index) => (
                        <option key={index} value={option}>
                            {option}
                        </option>
                    ))}
                </select>
            </div>

            <div className="form-group">
                <label htmlFor="newOption">Добавить новый вариант:</label>
                <div className="input-group">
                    <input
                        type="text"
                        id="newOption"
                        value={newOption}
                        onChange={(e) => setNewOption(e.target.value)}
                        className="form-control"
                        placeholder="Введите новый вариант"
                    />
                    <button
                        onClick={handleAddOption}
                        className="btn btn-add"
                        disabled={!newOption.trim()}
                    >
                        Добавить
                    </button>
                </div>
            </div>

            <button
                onClick={handleNavigate}
                className="btn btn-primary"
                disabled={!selectedOption}
            >
                Перейти далее
            </button>
        </div>
    );
}

export default SelectionPage;