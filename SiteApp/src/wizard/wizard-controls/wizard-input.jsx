import React from 'react';
import "./wizard-controls.css"

function WizardInput({ placeholder, multiRow, onChange, value, error }) {
   
    const handleChange = (value) => {
        onChange(value);
    }

    return (
        <div>
            {multiRow ?
                <div>
                    <div className='wm-input'>
                        <textarea
                            cols="1"
                            rows="5"
                            type="name"
                            placeholder={placeholder}
                            autoComplete="off"
                            onChange={(e) => handleChange(e.target.value)}
                            value={value}
                            required
                        ></textarea>
                    </div>
                    <p className='wm-error-field'>{error}</p>
                </div>
                :
                <div>
                    <div className='wm-input'>
                        <input
                            type="name"
                            placeholder={placeholder}
                            autoComplete="off"
                            onChange={(e) => handleChange(e.target.value)}
                            value={value}
                            required
                        ></input>
                    </div>
                    <p className='wm-error-field'>{error}</p>
                </div>
            }
        </div>
    );
}

export default WizardInput;
