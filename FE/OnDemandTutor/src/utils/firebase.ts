import { initializeApp } from 'firebase/app'
import { getStorage } from 'firebase/storage'

const firebaseConfig = {
  apiKey: 'AIzaSyAa6ecvr_RqxErRdMhZDwTOW8sbD76TxaY',
  authDomain: 'otdlearning.firebaseapp.com',
  projectId: 'otdlearning',
  storageBucket: 'otdlearning.appspot.com',
  messagingSenderId: '586070345525',
  appId: '1:586070345525:web:4f2ee8469b50398708ee7c',
  measurementId: 'G-L1S42M3GCQ'
}

// Initialize Firebase
const app = initializeApp(firebaseConfig)
export const storage = getStorage(app)
