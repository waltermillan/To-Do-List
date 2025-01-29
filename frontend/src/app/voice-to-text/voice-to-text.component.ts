import { Component, OnInit } from '@angular/core';

@Component(
{
  selector: 'app-voice-to-text',
  templateUrl: './voice-to-text.component.html',
  styleUrls: ['./voice-to-text.component.css']
})
export class VoiceToTextComponent implements OnInit 
{
  text: string = '';  // Texto capturado de la voz
  recognition: any;
  isRecognizing = false; // Indicador de si está reconociendo
  errorMessage: string | null = null;  // Mensaje de error para mostrar en la UI

  constructor() 
  {
    
  }

  ngOnInit(): void 
  {
    if (typeof window !== 'undefined' && ('SpeechRecognition' in window || 'webkitSpeechRecognition' in window)) 
    {
      this.recognition = new (window.SpeechRecognition || window.webkitSpeechRecognition)();
      this.recognition.lang = 'es-ES';  // Configurar el idioma
      this.recognition.continuous = false;  // Detenerse después de una frase
      this.recognition.interimResults = false;  // Mostrar solo resultados finales

      // Cuando se detecten resultados, actualiza el texto
      this.recognition.onresult = (event: any) => 
      {
        let finalTranscript = '';
        for (let i = event.resultIndex; i < event.results.length; i++)
        {
          const transcript = event.results[i][0].transcript;
          if (event.results[i].isFinal) 
          {
            finalTranscript += transcript + ' ';
          }
        }
        console.log('Texto final transcrito:', finalTranscript);
        this.text = finalTranscript;  // Actualiza el texto en la interfaz
      };

      // Manejar errores
      this.recognition.onerror = (event: any) => 
      {
        this.errorMessage = `Error: ${event.error}`;
      };

      // Cuando termine el reconocimiento, actualizamos la interfaz
      this.recognition.onend = () => {
        console.log('Reconocimiento detenido.');
        this.isRecognizing = false; // Solo se marca como no reconocedor cuando el reconocimiento ha terminado
      };
    } 
    else 
    {
      this.errorMessage = 'La API de reconocimiento de voz no está soportada en este navegador.';
    }
  }

  toggleRecognition(): void 
  {
    if (this.isRecognizing) 
    {
      this.stopRecognition();
    } 
    else 
    {
      this.startRecognition();
    }
  }

  startRecognition(): void 
  {
    // this.text = '';  // Limpiar el texto al empezar la grabación
    this.isRecognizing = true; // Establecer que estamos reconociendo
    this.recognition.start();  // Iniciar el reconocimiento de voz
    this.errorMessage = null;  // Limpiar cualquier mensaje de error
    console.log("Iniciando grabación...");
  }

  stopRecognition(): void 
  {
    if (this.isRecognizing) 
    {
      // Detener el reconocimiento
      this.recognition.stop();
      console.log('Deteniendo grabación...');
      this.isRecognizing = false; // Establecer que ya no estamos reconociendo
    }
  }
}
