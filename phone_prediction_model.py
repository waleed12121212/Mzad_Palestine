import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestRegressor
from sklearn.preprocessing import StandardScaler
import joblib

# إنشاء بيانات تدريب وهمية
def create_sample_data():
    np.random.seed(42)
    n_samples = 1000
    
    data = {
        'battery_capacity': np.random.randint(3000, 6000, n_samples),
        'display_size': np.random.uniform(5.0, 7.0, n_samples),
        'storage': np.random.choice([64, 128, 256, 512, 1024], n_samples),
        'ram': np.random.choice([4, 6, 8, 12, 16], n_samples),
        'refresh_rate': np.random.choice([60, 90, 120, 144], n_samples),
        'front_camera_mp': np.random.choice([8, 12, 16, 20, 32], n_samples),
        'rear_camera_mp': np.random.choice([12, 48, 64, 108, 200], n_samples),
        'charging_speed': np.random.choice([15, 25, 33, 45, 65, 100], n_samples)
    }
    
    # حساب السعر بناءً على المواصفات
    base_price = 1000
    price = (
        data['battery_capacity'] * 0.1 +
        data['display_size'] * 100 +
        data['storage'] * 0.5 +
        data['ram'] * 50 +
        data['refresh_rate'] * 2 +
        data['front_camera_mp'] * 10 +
        data['rear_camera_mp'] * 5 +
        data['charging_speed'] * 3 +
        base_price
    )
    
    data['price'] = price
    return pd.DataFrame(data)

# إنشاء وتدريب النموذج
def train_model():
    # إنشاء بيانات التدريب
    df = create_sample_data()
    
    # تحضير البيانات
    X = df.drop('price', axis=1)
    y = df['price']
    
    # تقسيم البيانات
    X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)
    
    # تحجيم البيانات
    scaler = StandardScaler()
    X_train_scaled = scaler.fit_transform(X_train)
    X_test_scaled = scaler.transform(X_test)
    
    # إنشاء وتدريب النموذج
    model = RandomForestRegressor(n_estimators=100, random_state=42)
    model.fit(X_train_scaled, y_train)
    
    # حفظ النموذج والمحول
    joblib.dump(model, 'phone_price_model.pkl')
    joblib.dump(scaler, 'phone_price_scaler.pkl')
    
    return model, scaler

# التنبؤ بالسعر
def predict_price(phone_specs):
    try:
        # تحميل النموذج والمحول
        model = joblib.load('phone_price_model.pkl')
        scaler = joblib.load('phone_price_scaler.pkl')
        
        # تحويل المواصفات إلى DataFrame
        specs_df = pd.DataFrame([phone_specs])
        
        # تحجيم البيانات
        specs_scaled = scaler.transform(specs_df)
        
        # التنبؤ بالسعر
        predicted_price = model.predict(specs_scaled)[0]
        
        return round(predicted_price, 2)
    except Exception as e:
        raise Exception(f"حدث خطأ أثناء التنبؤ بالسعر: {str(e)}")

if __name__ == "__main__":
    # تدريب النموذج
    train_model() 