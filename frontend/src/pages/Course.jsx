import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

function SelectionPage() {
    const navigate = useNavigate();
    const [selectedOption, setSelectedOption] = useState('');
    //добавление новых опций
    const [newOption, setNewOption] = useState('');
    //старые опции
    const [options, setOptions] = useState([]);
    const token = localStorage.getItem('token');

    const fetchCourses = useCallback(async () => {
        try {
            const response = await fetch('http://localhost:5249/courses/', {
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });
            if (!response.ok) {
                throw new Error('Ошибка при получении курсов');
            }
            const data = await response.json();
            setOptions(data);
            console.log('Курсы:', data);
        } catch (error) {
            console.error('Ошибка:', error);
        }
    }, [token]);

    useEffect(() => {
        if (token) {
          fetchCourses();
        }
      }, [fetchCourses, token]);

    // Добавление нового варианта
    // const handleAddOption = () => {
    //     if (newOption.trim() && !options.includes(newOption)) {
    //         setOptions([...options, newOption]);
    //         setNewOption('');
    //     }
    // };

    const handleAddOption = async () => {
        try {
            const response = await fetch('http://localhost:5249/courses/', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({ name: newOption }),
            });

            if (!response.ok) {
                throw new Error('Ошибка при добавлении группы');
            }

            const newCourse = await response.json();
            setOptions([...options, newCourse]);
            setNewOption('');
            fetchCourses();
            console.log('Курс добавлен:', newCourse);
        } catch (error) {
            console.error('Ошибка:', error);
        }
    };

    // Переход на следующую страницу
    const handleNavigate = () => {
        if (selectedOption) {
            navigate('/group', { state: { selectedOption } });
        }
    };

    return (
        <div className="selection-container">
            <h2>Выбери или добавь курс</h2>

            <div className="form-group">
                <label htmlFor="options">Курсы</label>
                <select
                    id="options"
                    value={selectedOption}
                    onChange={(e) => setSelectedOption(e.target.value)}
                    className="form-control"
                >
                    <option value="">тыкни на меня</option>
                    {options.map((option, index) => (
                        <option key={index} value={option.name}>
                            {option.name}
                        </option>
                    ))}
                </select>
            </div>

            <div className="form-group">
                <label htmlFor="newOption">Добавить новый курс</label>
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