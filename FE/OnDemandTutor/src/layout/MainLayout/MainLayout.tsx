import React, { useState } from 'react'
import Header from '../../components/Header'
import Footer from '../../components/Footer'

interface Props {
  children?: React.ReactNode
}

export default function MainLayout({ children }: Props) {
  return (
    <div className='w-full mx-auto my-auto flex flex-col items-center justify-center min-h-screen rounded-3xl'>
      <Header />
      {children}
      <Footer />
    </div>
  )
}
