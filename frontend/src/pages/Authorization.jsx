import {useState} from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles/Authorization.css';

function Authorization() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        try {
            console.log(username, password);
            const response = await fetch('http://130.193.59.231:5249/auth/', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ username, password }),
            });

            if (response.status === 401) {
                setError('Сессия истекла или токен недействителен. Повторите вход.');
                return;
            }

            if (!response.ok) {
                setError('Неверный логин или пароль');
                return;
            }

            const data = await response.json();
            const token = data.token;

            if (token) {
                localStorage.setItem('token', token);
                navigate('/courses');
            } else {
                setError('Не удалось получить токен');
            }

        } catch (err) {
            console.error('Ошибка при авторизации:', err);
            setError('Ошибка подключения к серверу');
        }
    }

    return (
        <div className="login-form">
            <h2>Вход</h2>
            <form onSubmit={handleSubmit}>
                <div className='form-group'>
                    <label htmlFor='username'>Почта</label>
                    <input
                        type='text'
                        id='username'
                        name='username'
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                        autoComplete="username"/>
                </div>
                <div className='form-group'>
                    <label htmlFor='password'>Пароль</label>
                    <input
                        type='password'
                        id='password'
                        name='password'
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                        autoComplete="current-password"/>
                </div>
                {error && <div className="error-message">{error}</div>}
                <button type="submit" className="login-button">Войти</button>
            </form>
        </div>
    );
}


export default Authorization;