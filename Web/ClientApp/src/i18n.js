import i18n from 'i18next';
import XHR from 'i18next-xhr-backend';
import LanguageDetector from 'i18next-browser-languagedetector';

i18n.use(XHR)
    .use(LanguageDetector)
    .init({
        lng: 'en',
        fallbackLng: 'en',
        debug: process.env.DEBUG_I18N
    });

export default i18n;